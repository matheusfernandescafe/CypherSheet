using Microsoft.JSInterop;

namespace CypherSheet.Client.Services;

public interface IThemeService
{
    bool IsDarkMode { get; }
    event Action? OnThemeChanged;
    Task InitializeAsync();
    Task SetDarkModeAsync(bool isDarkMode);
}

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode = false;

    public bool IsDarkMode => _isDarkMode;
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var storedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "cyphersheet-theme");
            _isDarkMode = storedTheme == "dark";
        }
        catch
        {
            // Se houver erro ao acessar localStorage, usar tema claro como padrão
            _isDarkMode = false;
        }
    }

    public async Task SetDarkModeAsync(bool isDarkMode)
    {
        if (_isDarkMode != isDarkMode)
        {
            _isDarkMode = isDarkMode;
            
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "cyphersheet-theme", isDarkMode ? "dark" : "light");
            }
            catch
            {
                // Ignorar erros de localStorage
            }
            
            OnThemeChanged?.Invoke();
        }
    }
}