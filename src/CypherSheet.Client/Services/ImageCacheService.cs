using CypherSheet.Storage;
using System.Collections.Concurrent;

namespace CypherSheet.Client.Services;

public class ImageCacheService : IImageCacheService
{
    private readonly ICharacterRepository _repository;
    private readonly ConcurrentDictionary<Guid, byte[]?> _imageCache = new();
    private readonly ConcurrentDictionary<Guid, Task<byte[]?>> _loadingTasks = new();
    private readonly SemaphoreSlim _semaphore = new(5, 5); // Limita carregamentos simultâneos

    public ImageCacheService(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]?> GetImageAsync(Guid characterId)
    {
        // Retorna do cache se disponível
        if (_imageCache.TryGetValue(characterId, out var cachedImage))
        {
            return cachedImage;
        }

        // Se já está sendo carregada, aguarda a task existente
        if (_loadingTasks.TryGetValue(characterId, out var existingTask))
        {
            return await existingTask;
        }

        // Inicia novo carregamento com retry
        var loadingTask = LoadImageWithRetryAsync(characterId);
        _loadingTasks[characterId] = loadingTask;

        try
        {
            var result = await loadingTask;
            _imageCache[characterId] = result;
            return result;
        }
        finally
        {
            _loadingTasks.TryRemove(characterId, out _);
        }
    }

    private async Task<byte[]?> LoadImageWithRetryAsync(Guid characterId)
    {
        const int maxRetries = 3;
        var retryCount = 0;
        
        while (retryCount < maxRetries)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    var imageData = await _repository.GetCharacterImageAsync(characterId);
                    
                    // Validar se os dados da imagem são válidos
                    if (imageData != null && imageData.Length > 0)
                    {
                        // Verificação básica de integridade
                        if (IsValidImageData(imageData))
                        {
                            return imageData;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid image data for character {characterId}, removing from storage");
                            // Remover dados corrompidos
                            await _repository.DeleteCharacterImageAsync(characterId);
                            return null;
                        }
                    }
                    
                    return null;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (Exception ex) when (retryCount < maxRetries - 1)
            {
                retryCount++;
                Console.WriteLine($"Attempt {retryCount} failed loading image for character {characterId}: {ex.Message}");
                
                // Aguardar com backoff exponencial
                var delay = (int)Math.Pow(2, retryCount - 1) * 1000;
                await Task.Delay(delay);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Final attempt failed loading image for character {characterId}: {ex.Message}");
                
                // Cache o resultado nulo para evitar tentativas repetidas
                _imageCache[characterId] = null;
                return null;
            }
        }
        
        // Se todas as tentativas falharam
        Console.WriteLine($"All retry attempts failed for character {characterId}");
        _imageCache[characterId] = null;
        return null;
    }

    private static bool IsValidImageData(byte[] imageData)
    {
        if (imageData == null || imageData.Length < 10)
            return false;

        // Verificar magic bytes para formatos suportados
        // JPEG: FF D8 FF
        if (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
            return true;

        // PNG: 89 50 4E 47 0D 0A 1A 0A
        if (imageData.Length >= 8 && 
            imageData[0] == 0x89 && imageData[1] == 0x50 && 
            imageData[2] == 0x4E && imageData[3] == 0x47 &&
            imageData[4] == 0x0D && imageData[5] == 0x0A && 
            imageData[6] == 0x1A && imageData[7] == 0x0A)
            return true;

        // WebP: RIFF....WEBP
        if (imageData.Length >= 12 &&
            imageData[0] == 0x52 && imageData[1] == 0x49 && 
            imageData[2] == 0x46 && imageData[3] == 0x46 &&
            imageData[8] == 0x57 && imageData[9] == 0x45 && 
            imageData[10] == 0x42 && imageData[11] == 0x50)
            return true;

        return false;
    }

    public void CacheImage(Guid characterId, byte[]? imageData)
    {
        _imageCache[characterId] = imageData;
    }

    public void ClearCache()
    {
        _imageCache.Clear();
        _loadingTasks.Clear();
    }

    public void RemoveFromCache(Guid characterId)
    {
        _imageCache.TryRemove(characterId, out _);
        _loadingTasks.TryRemove(characterId, out _);
    }

    public bool IsImageCached(Guid characterId)
    {
        return _imageCache.ContainsKey(characterId);
    }

    public bool IsImageLoading(Guid characterId)
    {
        return _loadingTasks.ContainsKey(characterId);
    }

    public async Task PreloadImagesAsync(IEnumerable<Guid> characterIds)
    {
        var preloadTasks = characterIds
            .Where(id => !IsImageCached(id) && !IsImageLoading(id))
            .Select(async characterId =>
            {
                try
                {
                    await GetImageAsync(characterId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error preloading image for character {characterId}: {ex.Message}");
                }
            });

        await Task.WhenAll(preloadTasks);
    }
}