using System.Reflection;

namespace CypherSheet.Domain;

public static class AppVersion
{
    public const string Version = "1.0.0";
    public static string BuildDate => GetBuildDate();
    public static string FullVersion => $"v{Version} ({BuildDate})";
    private static string GetBuildDate()
    {
        try
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            
            // Try to get build date from assembly attributes first
            var buildDateAttribute = assembly.GetCustomAttribute<System.Reflection.AssemblyMetadataAttribute>();
            if (buildDateAttribute?.Key == "BuildDate")
            {
                return buildDateAttribute.Value ?? DateTime.Now.ToString("yyyy-MM-dd");
            }

            // Fallback: Use current date for Blazor WebAssembly
            // In production, this should be replaced by build-time injection
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        catch
        {
            // Ultimate fallback
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    public static int Major => int.Parse(Version.Split('.')[0]);
    public static int Minor => int.Parse(Version.Split('.')[1]);
    public static int Patch => int.Parse(Version.Split('.')[2]);
}