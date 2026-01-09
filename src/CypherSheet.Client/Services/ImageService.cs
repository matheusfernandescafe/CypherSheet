using Microsoft.JSInterop;

namespace CypherSheet.Client.Services;

public class ImageService : IImageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly INotificationService _notificationService;

    // Tipos de arquivo permitidos
    private static readonly HashSet<string> AllowedMimeTypes = new()
    {
        "image/jpeg",
        "image/jpg", 
        "image/png",
        "image/webp"
    };

    // Extensões permitidas
    private static readonly HashSet<string> AllowedExtensions = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    // Tamanho máximo: 5MB
    private const long MaxFileSize = 5 * 1024 * 1024;

    public ImageService(IJSRuntime jsRuntime, INotificationService notificationService)
    {
        _jsRuntime = jsRuntime;
        _notificationService = notificationService;
    }

    public ImageValidationResult ValidateImage(string fileName, string contentType, long fileSize)
    {
        var errors = new List<string>();

        // Validar nome do arquivo
        if (string.IsNullOrWhiteSpace(fileName))
        {
            errors.Add("Nome do arquivo é obrigatório");
            return ImageValidationResult.Failure(errors);
        }

        // Validar caracteres perigosos no nome do arquivo
        var invalidChars = Path.GetInvalidFileNameChars();
        if (fileName.IndexOfAny(invalidChars) >= 0)
        {
            errors.Add("Nome do arquivo contém caracteres inválidos");
        }

        // Validar tamanho do nome do arquivo
        if (fileName.Length > 255)
        {
            errors.Add("Nome do arquivo é muito longo (máximo 255 caracteres)");
        }

        // Validar tamanho do arquivo
        if (fileSize <= 0)
        {
            errors.Add("O arquivo está vazio ou corrompido");
        }
        else if (fileSize > MaxFileSize)
        {
            var maxSizeMB = MaxFileSize / (1024.0 * 1024.0);
            var currentSizeMB = fileSize / (1024.0 * 1024.0);
            errors.Add($"Arquivo muito grande ({currentSizeMB:F1}MB). Tamanho máximo: {maxSizeMB:F0}MB");
        }

        // Validar tipo MIME
        if (string.IsNullOrWhiteSpace(contentType))
        {
            errors.Add("Tipo de conteúdo do arquivo não foi fornecido");
        }
        else
        {
            var normalizedContentType = contentType.ToLowerInvariant().Trim();
            if (!AllowedMimeTypes.Contains(normalizedContentType))
            {
                errors.Add($"Tipo de arquivo não suportado ({contentType}). Tipos permitidos: JPEG, PNG, WebP");
            }
        }

        // Validar extensão do arquivo
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension))
        {
            errors.Add("Arquivo deve ter uma extensão válida");
        }
        else if (!AllowedExtensions.Contains(extension))
        {
            errors.Add($"Extensão não suportada ({extension}). Extensões permitidas: .jpg, .jpeg, .png, .webp");
        }

        // Validar consistência entre MIME type e extensão
        if (!string.IsNullOrWhiteSpace(contentType) && !string.IsNullOrEmpty(extension))
        {
            var isConsistent = ValidateMimeTypeExtensionConsistency(contentType.ToLowerInvariant(), extension);
            if (!isConsistent)
            {
                errors.Add($"Inconsistência entre tipo de arquivo ({contentType}) e extensão ({extension})");
            }
        }

        return errors.Count == 0 
            ? ImageValidationResult.Success() 
            : ImageValidationResult.Failure(errors);
    }

    private static bool ValidateMimeTypeExtensionConsistency(string mimeType, string extension)
    {
        return mimeType switch
        {
            "image/jpeg" or "image/jpg" => extension is ".jpg" or ".jpeg",
            "image/png" => extension == ".png",
            "image/webp" => extension == ".webp",
            _ => false
        };
    }

    public ImageValidationResult ValidateImageContent(byte[] imageData, string fileName, string contentType)
    {
        var errors = new List<string>();

        if (imageData == null || imageData.Length == 0)
        {
            errors.Add("Dados da imagem estão vazios");
            return ImageValidationResult.Failure(errors);
        }

        // Validar magic bytes (assinatura do arquivo)
        var detectedFormat = DetectImageFormat(imageData);
        if (detectedFormat == ImageFormat.Unknown)
        {
            errors.Add("Arquivo não é uma imagem válida ou formato não suportado");
        }
        else
        {
            // Verificar se o formato detectado é consistente com a extensão
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var isFormatConsistent = ValidateFormatConsistency(detectedFormat, extension);
            if (!isFormatConsistent)
            {
                errors.Add($"Formato real da imagem não corresponde à extensão do arquivo ({extension})");
            }
        }

        // Validar tamanho mínimo (uma imagem válida deve ter pelo menos alguns bytes)
        if (imageData.Length < 100)
        {
            errors.Add("Arquivo muito pequeno para ser uma imagem válida");
        }

        return errors.Count == 0 
            ? ImageValidationResult.Success() 
            : ImageValidationResult.Failure(errors);
    }

    private static ImageFormat DetectImageFormat(byte[] imageData)
    {
        if (imageData.Length < 4) return ImageFormat.Unknown;

        // JPEG: FF D8 FF
        if (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
            return ImageFormat.Jpeg;

        // PNG: 89 50 4E 47 0D 0A 1A 0A
        if (imageData.Length >= 8 && 
            imageData[0] == 0x89 && imageData[1] == 0x50 && 
            imageData[2] == 0x4E && imageData[3] == 0x47 &&
            imageData[4] == 0x0D && imageData[5] == 0x0A && 
            imageData[6] == 0x1A && imageData[7] == 0x0A)
            return ImageFormat.Png;

        // WebP: RIFF....WEBP
        if (imageData.Length >= 12 &&
            imageData[0] == 0x52 && imageData[1] == 0x49 && 
            imageData[2] == 0x46 && imageData[3] == 0x46 &&
            imageData[8] == 0x57 && imageData[9] == 0x45 && 
            imageData[10] == 0x42 && imageData[11] == 0x50)
            return ImageFormat.WebP;

        return ImageFormat.Unknown;
    }

    private static bool ValidateFormatConsistency(ImageFormat format, string extension)
    {
        return format switch
        {
            ImageFormat.Jpeg => extension is ".jpg" or ".jpeg",
            ImageFormat.Png => extension == ".png",
            ImageFormat.WebP => extension == ".webp",
            _ => false
        };
    }

    private enum ImageFormat
    {
        Unknown,
        Jpeg,
        Png,
        WebP
    }

    public async Task<byte[]> ProcessImageAsync(byte[] imageData, int maxWidth = 800, int maxHeight = 800, double quality = 0.85)
    {
        const int maxRetries = 3;
        var retryCount = 0;
        
        while (retryCount < maxRetries)
        {
            try
            {
                // Converter para base64 para enviar ao JavaScript
                var base64Image = Convert.ToBase64String(imageData);
                
                // Chamar função JavaScript para processar a imagem
                var processedBase64 = await _jsRuntime.InvokeAsync<string>(
                    "imageProcessor.processImage", 
                    base64Image, 
                    maxWidth, 
                    maxHeight, 
                    quality);

                // Converter de volta para bytes
                return Convert.FromBase64String(processedBase64);
            }
            catch (JSException) when (retryCount < maxRetries - 1)
            {
                retryCount++;
                _notificationService.ShowWarning($"Tentativa {retryCount} de processamento falhou, tentando novamente...");
                
                // Aguardar um pouco antes de tentar novamente
                await Task.Delay(1000 * retryCount);
                
                // Se for a última tentativa, tentar com qualidade reduzida
                if (retryCount == maxRetries - 1)
                {
                    quality = Math.Max(0.5, quality - 0.2);
                    maxWidth = Math.Min(maxWidth, 600);
                    maxHeight = Math.Min(maxHeight, 600);
                    _notificationService.ShowInfo("Tentando processar com qualidade reduzida...");
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Erro ao processar imagem: {ex.Message}");
                
                // Se falhar completamente, tentar retornar a imagem original se for pequena o suficiente
                if (imageData.Length <= MaxFileSize / 2)
                {
                    _notificationService.ShowWarning("Retornando imagem original sem processamento");
                    return imageData;
                }
                
                throw;
            }
        }
        
        // Se todas as tentativas falharam, tentar retornar imagem original
        if (imageData.Length <= MaxFileSize / 2)
        {
            _notificationService.ShowWarning("Processamento falhou, usando imagem original");
            return imageData;
        }
        
        throw new InvalidOperationException("Não foi possível processar a imagem após múltiplas tentativas");
    }

    public async Task<byte[]> GenerateThumbnailAsync(byte[] imageData, int size = 150)
    {
        const int maxRetries = 2;
        var retryCount = 0;
        
        while (retryCount < maxRetries)
        {
            try
            {
                // Usar o método de processamento com tamanho fixo para thumbnail
                return await ProcessImageAsync(imageData, size, size, 0.8);
            }
            catch (Exception) when (retryCount < maxRetries - 1)
            {
                retryCount++;
                _notificationService.ShowWarning($"Falha ao gerar thumbnail, tentativa {retryCount + 1}...");
                
                // Aguardar antes de tentar novamente
                await Task.Delay(500 * retryCount);
                
                // Na última tentativa, usar tamanho menor
                if (retryCount == maxRetries - 1)
                {
                    size = Math.Min(size, 100);
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Erro ao gerar thumbnail: {ex.Message}");
                
                // Fallback: retornar imagem original se for pequena
                if (imageData.Length <= 50 * 1024) // 50KB
                {
                    _notificationService.ShowInfo("Usando imagem original como thumbnail");
                    return imageData;
                }
                
                throw;
            }
        }
        
        // Se falhar, tentar retornar imagem original reduzida
        try
        {
            return await ProcessImageAsync(imageData, 75, 75, 0.6);
        }
        catch
        {
            // Último recurso: retornar imagem original se for pequena
            if (imageData.Length <= 100 * 1024) // 100KB
            {
                return imageData;
            }
            
            throw new InvalidOperationException("Não foi possível gerar thumbnail");
        }
    }

    public async Task<byte[]> ConvertToWebPIfSupportedAsync(byte[] imageData, double quality = 0.85)
    {
        try
        {
            var isWebPSupported = await IsWebPSupportedAsync();
            
            if (!isWebPSupported)
            {
                return imageData; // Retorna a imagem original se WebP não for suportado
            }

            var base64Image = Convert.ToBase64String(imageData);
            
            var convertedBase64 = await _jsRuntime.InvokeAsync<string>(
                "imageProcessor.convertToWebP", 
                base64Image, 
                quality);

            return Convert.FromBase64String(convertedBase64);
        }
        catch (Exception ex)
        {
            _notificationService.ShowWarning($"Não foi possível converter para WebP: {ex.Message}");
            return imageData; // Retorna a imagem original em caso de erro
        }
    }

    public async Task<bool> IsWebPSupportedAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<bool>("imageProcessor.isWebPSupported");
        }
        catch
        {
            return false; // Assume que não é suportado em caso de erro
        }
    }
}