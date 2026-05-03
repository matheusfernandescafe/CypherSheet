using CypherSheet.Client.Pages;

namespace CypherSheet.Client.Services;

/// <summary>
/// Serviço de estado de navegação de UI. Compartilha a tab ativa
/// entre MainLayout, SidebarNavigation, AppFooter e as páginas.
/// </summary>
public class NavigationStateService
{
    private TabType _currentTab = TabType.Characters;

    /// <summary>
    /// Obtém a tab atualmente ativa.
    /// </summary>
    public TabType CurrentTab => _currentTab;

    /// <summary>
    /// Disparado quando a tab ativa muda.
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Define a tab ativa e notifica os assinantes.
    /// </summary>
    /// <param name="tab">A tab a ser ativada.</param>
    public void SetTab(TabType tab)
    {
        // Validação de enum
        if (!Enum.IsDefined(typeof(TabType), tab))
        {
            return;
        }

        // Verifica se é diferente da tab atual
        if (_currentTab == tab)
        {
            return;
        }

        _currentTab = tab;
        OnChange?.Invoke();
    }
}
