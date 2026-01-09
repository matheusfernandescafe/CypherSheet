using MudBlazor;

namespace CypherSheet.Client.Services;

public interface INotificationService
{
    /// <summary>
    /// Exibe uma notificação de sucesso
    /// </summary>
    void ShowSuccess(string message, string? title = null, int? duration = null);
    
    /// <summary>
    /// Exibe uma notificação de erro
    /// </summary>
    void ShowError(string message, string? title = null, int? duration = null);
    
    /// <summary>
    /// Exibe uma notificação de aviso
    /// </summary>
    void ShowWarning(string message, string? title = null, int? duration = null);
    
    /// <summary>
    /// Exibe uma notificação informativa
    /// </summary>
    void ShowInfo(string message, string? title = null, int? duration = null);
    
    /// <summary>
    /// Exibe uma notificação de operação em progresso
    /// </summary>
    void ShowProgress(string message, string? title = null);
    
    /// <summary>
    /// Exibe uma notificação de confirmação para operações críticas
    /// </summary>
    void ShowCriticalSuccess(string message, string? title = null);
    
    /// <summary>
    /// Exibe uma notificação de erro crítico
    /// </summary>
    void ShowCriticalError(string message, string? title = null);
    
    /// <summary>
    /// Exibe uma notificação personalizada
    /// </summary>
    void ShowCustom(string message, Severity severity, string? icon = null, string? title = null, int? duration = null);
}