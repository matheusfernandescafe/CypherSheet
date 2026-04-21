using System.Reflection;

namespace CypherSheet.Domain;

public static class AppVersion
{
    public const string Version = "1.5.1";
    public static string BuildDate => GetBuildDate();
    public static string FullVersion => $"v{Version} ({BuildDate})";
    private static string GetBuildDate()
    {
        try
        {
            var assembly = typeof(AppVersion).Assembly;

            var buildDateAttribute = assembly
                .GetCustomAttributes<System.Reflection.AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "BuildDate");

            if (buildDateAttribute?.Value is { Length: > 0 } value)
            {
                return value;
            }

            return "dev";
        }
        catch
        {
            return "dev";
        }
    }

    public static int Major => int.Parse(Version.Split('.')[0]);
    public static int Minor => int.Parse(Version.Split('.')[1]);
    public static int Patch => int.Parse(Version.Split('.')[2]);
}