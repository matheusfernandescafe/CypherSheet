namespace CypherSheet.Domain;

public class ExportFormat
{
    public ExportMetadata Metadata { get; set; } = new();
    public List<Character> Characters { get; set; } = new();
}

public class ExportMetadata
{
    public string AppVersion { get; set; } = string.Empty;
    public DateTime ExportDate { get; set; }
    public int CharacterCount { get; set; }
    public string FormatVersion { get; set; } = "1.0";

    public ExportMetadata()
    {
        AppVersion = Domain.AppVersion.Version;
        ExportDate = DateTime.UtcNow;
    }
}