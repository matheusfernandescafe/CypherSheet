using MudBlazor;

namespace CypherSheet.Client.Services;

public class NotificationService : INotificationService
{
    private readonly ISnackbar _snackbar;

    public NotificationService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void ShowSuccess(string message, string? title = null, int? duration = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Success, config =>
        {
            config.Icon = Icons.Material.Filled.CheckCircle;
            config.VisibleStateDuration = duration ?? 3000;
            config.ShowCloseIcon = true;
        });
    }

    public void ShowError(string message, string? title = null, int? duration = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Error, config =>
        {
            config.Icon = Icons.Material.Filled.Error;
            config.VisibleStateDuration = duration ?? 5000;
            config.ShowCloseIcon = true;
        });
    }

    public void ShowWarning(string message, string? title = null, int? duration = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Warning, config =>
        {
            config.Icon = Icons.Material.Filled.Warning;
            config.VisibleStateDuration = duration ?? 4000;
            config.ShowCloseIcon = true;
        });
    }

    public void ShowInfo(string message, string? title = null, int? duration = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Info, config =>
        {
            config.Icon = Icons.Material.Filled.Info;
            config.VisibleStateDuration = duration ?? 3000;
            config.ShowCloseIcon = true;
        });
    }

    public void ShowProgress(string message, string? title = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Info, config =>
        {
            config.Icon = Icons.Material.Filled.Sync;
            config.VisibleStateDuration = 10000; // Mais tempo para operações em progresso
            config.ShowCloseIcon = false; // Não permitir fechar durante progresso
        });
    }

    public void ShowCriticalSuccess(string message, string? title = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Success, config =>
        {
            config.Icon = Icons.Material.Filled.Verified;
            config.VisibleStateDuration = 6000; // Mais tempo para operações críticas
            config.ShowCloseIcon = true;
        });
    }

    public void ShowCriticalError(string message, string? title = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, Severity.Error, config =>
        {
            config.Icon = Icons.Material.Filled.ErrorOutline;
            config.VisibleStateDuration = 8000; // Mais tempo para erros críticos
            config.ShowCloseIcon = true;
        });
    }

    public void ShowCustom(string message, Severity severity, string? icon = null, string? title = null, int? duration = null)
    {
        var displayMessage = FormatMessage(title, message);
        _snackbar.Add(displayMessage, severity, config =>
        {
            if (!string.IsNullOrEmpty(icon))
                config.Icon = icon;
            
            config.VisibleStateDuration = duration ?? GetDefaultDuration(severity);
            config.ShowCloseIcon = true;
        });
    }

    private static string FormatMessage(string? title, string message)
    {
        if (string.IsNullOrEmpty(title))
            return message;
        
        return $"<strong>{title}</strong><br/>{message}";
    }

    private static int GetDefaultDuration(Severity severity)
    {
        return severity switch
        {
            Severity.Success => 3000,
            Severity.Info => 3000,
            Severity.Warning => 4000,
            Severity.Error => 5000,
            _ => 3000
        };
    }
}