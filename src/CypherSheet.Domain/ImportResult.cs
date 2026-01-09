namespace CypherSheet.Domain;

public class ImportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Warnings { get; set; } = new();
    public int ImportedCount { get; set; }
    public bool VersionMismatch { get; set; }
    public DataManagementError ErrorCode { get; set; } = DataManagementError.None;
}