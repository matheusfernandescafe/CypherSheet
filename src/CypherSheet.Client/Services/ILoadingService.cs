namespace CypherSheet.Client.Services;

public interface ILoadingService
{
    /// <summary>
    /// Indica se há alguma operação em progresso
    /// </summary>
    bool IsLoading { get; }
    
    /// <summary>
    /// Obtém a mensagem de loading atual
    /// </summary>
    string? LoadingMessage { get; }
    
    /// <summary>
    /// Obtém o progresso atual (0-100)
    /// </summary>
    int Progress { get; }
    
    /// <summary>
    /// Evento disparado quando o estado de loading muda
    /// </summary>
    event Action? LoadingStateChanged;
    
    /// <summary>
    /// Inicia uma operação de loading
    /// </summary>
    void StartLoading(string? message = null);
    
    /// <summary>
    /// Atualiza a mensagem de loading
    /// </summary>
    void UpdateLoadingMessage(string message);
    
    /// <summary>
    /// Atualiza o progresso da operação
    /// </summary>
    void UpdateProgress(int progress);
    
    /// <summary>
    /// Para a operação de loading
    /// </summary>
    void StopLoading();
    
    /// <summary>
    /// Executa uma operação com loading automático
    /// </summary>
    Task ExecuteWithLoadingAsync(Func<Task> operation, string? message = null);
    
    /// <summary>
    /// Executa uma operação com loading automático e retorna resultado
    /// </summary>
    Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> operation, string? message = null);
}