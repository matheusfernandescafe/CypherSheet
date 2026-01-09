using CypherSheet.Shared;
using Microsoft.JSInterop;

namespace CypherSheet.Client.Services;

public class CacheService(IJSRuntime jsRuntime) : ICacheService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task ClearApplicationCacheAsync()
        {
            try
            {
                // Limpar Service Worker cache
                await _jsRuntime.InvokeVoidAsync("eval", @"
                    if ('caches' in window) {
                        caches.keys().then(function(names) {
                            for (let name of names) {
                                caches.delete(name);
                            }
                        });
                    }
                ");

                // Limpar localStorage (exceto dados de personagens que ficam no IndexedDB)
                await _jsRuntime.InvokeVoidAsync("eval", @"
                    if (typeof(Storage) !== 'undefined') {
                        // Preservar apenas dados críticos se necessário
                        localStorage.clear();
                        sessionStorage.clear();
                    }
                ");
            }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao limpar cache da aplicação: {ex.Message}", ex);
        }
    }

    public async Task<bool> IsCacheAvailableAsync()
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<bool>("eval", @"
                'caches' in window && typeof(Storage) !== 'undefined'
            ");
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}