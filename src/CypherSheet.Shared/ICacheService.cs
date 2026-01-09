namespace CypherSheet.Shared;

public interface ICacheService
{
    Task ClearApplicationCacheAsync();
    Task<bool> IsCacheAvailableAsync();
}