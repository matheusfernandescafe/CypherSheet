namespace CypherSheet.Client.Services;

public interface IImageService
{
    /// <summary>
    /// Valida se o arquivo é uma imagem válida
    /// </summary>
    /// <param name="fileName">Nome do arquivo</param>
    /// <param name="contentType">Tipo de conteúdo MIME</param>
    /// <param name="fileSize">Tamanho do arquivo em bytes</param>
    /// <returns>Resultado da validação</returns>
    ImageValidationResult ValidateImage(string fileName, string contentType, long fileSize);

    /// <summary>
    /// Valida o conteúdo da imagem verificando magic bytes e consistência de formato
    /// </summary>
    /// <param name="imageData">Dados binários da imagem</param>
    /// <param name="fileName">Nome do arquivo</param>
    /// <param name="contentType">Tipo de conteúdo MIME</param>
    /// <returns>Resultado da validação de conteúdo</returns>
    ImageValidationResult ValidateImageContent(byte[] imageData, string fileName, string contentType);

    /// <summary>
    /// Processa uma imagem (redimensiona e comprime)
    /// </summary>
    /// <param name="imageData">Dados da imagem original</param>
    /// <param name="maxWidth">Largura máxima (padrão: 800px)</param>
    /// <param name="maxHeight">Altura máxima (padrão: 800px)</param>
    /// <param name="quality">Qualidade da compressão (0.0 a 1.0, padrão: 0.85)</param>
    /// <returns>Dados da imagem processada</returns>
    Task<byte[]> ProcessImageAsync(byte[] imageData, int maxWidth = 800, int maxHeight = 800, double quality = 0.85);

    /// <summary>
    /// Gera um thumbnail da imagem
    /// </summary>
    /// <param name="imageData">Dados da imagem original</param>
    /// <param name="size">Tamanho do thumbnail (padrão: 150px)</param>
    ///  /// <returns>umbnail</returns>
    Task<byte[]> GenerateThumbnailAsync(byte[] imageData, int size = 150);

    /// <summary>
    /// Converte imagem para formato WebP se suportado pelo navegador
    /// </summary>
    /// <param name="imageData">Dados da imagem original</param>
    /// <param name="quality">Qualidade da conversão (0.0 a 1.0, padrão: 0.85)</param>
    /// <returns>Dados da imagem convertida ou original se WebP não for suportado</returns>
    Task<byte[]> ConvertToWebPIfSupportedAsync(byte[] imageData, double quality = 0.85);

    /// <summary>
    /// Verifica se o navegador suporta formato WebP
    /// </summary>
    /// <returns>True se WebP for suportado</returns>
    Task<bool> IsWebPSupportedAsync();
}

public class ImageValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ImageValidationResult Success() => new() { IsValid = true };
    
    public static ImageValidationResult Failure(string errorMessage) => new() 
    { 
        IsValid = false, 
        ErrorMessage = errorMessage,
        Errors = new List<string> { errorMessage }
    };
    
    public static ImageValidationResult Failure(List<string> errors) => new() 
    { 
        IsValid = false, 
        ErrorMessage = string.Join("; ", errors),
        Errors = errors
    };
}