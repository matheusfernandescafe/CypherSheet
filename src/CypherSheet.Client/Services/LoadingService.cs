namespace CypherSheet.Client.Services;

public class LoadingService : ILoadingService
{
    private bool _isLoading;
    private string? _loadingMessage;
    private int _progress;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                LoadingStateChanged?.Invoke();
            }
        }
    }

    public string? LoadingMessage
    {
        get => _loadingMessage;
        private set
        {
            if (_loadingMessage != value)
            {
                _loadingMessage = value;
                LoadingStateChanged?.Invoke();
            }
        }
    }

    public int Progress
    {
        get => _progress;
        private set
        {
            if (_progress != value)
            {
                _progress = Math.Clamp(value, 0, 100);
                LoadingStateChanged?.Invoke();
            }
        }
    }

    public event Action? LoadingStateChanged;

    public void StartLoading(string? message = null)
    {
        LoadingMessage = message ?? "Carregando...";
        Progress = 0;
        IsLoading = true;
    }

    public void UpdateLoadingMessage(string message)
    {
        LoadingMessage = message;
    }

    public void UpdateProgress(int progress)
    {
        Progress = progress;
    }

    public void StopLoading()
    {
        IsLoading = false;
        LoadingMessage = null;
        Progress = 0;
    }

    public async Task ExecuteWithLoadingAsync(Func<Task> operation, string? message = null)
    {
        StartLoading(message);
        try
        {
            await operation();
        }
        finally
        {
            StopLoading();
        }
    }

    public async Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> operation, string? message = null)
    {
        StartLoading(message);
        try
        {
            return await operation();
        }
        finally
        {
            StopLoading();
        }
    }
}