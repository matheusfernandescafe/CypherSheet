namespace CypherSheet.Client.Services;

public interface IImageCacheService
{
    Task<byte[]?> GetImageAsync(Guid characterId);
    void CacheImage(Guid characterId, byte[]? imageData);
    void ClearCache();
    void RemoveFromCache(Guid characterId);
    bool IsImageCached(Guid characterId);
    bool IsImageLoading(Guid characterId);
    Task PreloadImagesAsync(IEnumerable<Guid> characterIds);
}