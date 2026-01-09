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
        _snackbar.Add(message, Severity.Success);
    }

    public void ShowError(string message, string? title = null, int? duration = null)
    {
        _snackbar.Add(message, Severity.Error);
    }

    public void ShowInfo(string message, string? title = null, int? duration = null)
    {
        _snackbar.Add(message, Severity.Info);
    }

    public void ShowWarning(string message, string? title = null, int? duration = null)
    {
        _snackbar.Add(message, Severity.Warning);
    }

    public void ShowProgress(string message, string? title = null)
    {
        _snackbar.Add(message, Severity.Info);
    }

    public void ShowCriticalSuccess(string message, string? title = null)
    {
        _snackbar.Add(message, Severity.Success);
    }

    public void ShowCriticalError(string message, string? title = null)
    {
        _snackbar.Add(message, Severity.Error);
    }

    public void ShowCustom(string message, Severity severity, string? icon = null, string? title = null, int? duration = null)
    {
        _snackbar.Add(message, severity);
    }
}