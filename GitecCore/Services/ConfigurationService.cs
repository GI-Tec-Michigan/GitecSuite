using Gitec.Utilities;
using Newtonsoft.Json;

namespace Gitec.Services;

public static class ConfigurationService
{
    public static bool IsInitialized { get; set; } = false;
    private static string? BasePath { get; set; }
    private static string? LogPath { get; set; }
    private static string? LogFile { get; set; }
    private static string? DatabasePath { get; set; }
    private static string? AppName { get; set; }
    private static string? DatabaseFile { get; set; }

    
    public static string GetAppName()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return AppName!;
    }
    
    public static string GetBasePath()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return BasePath!;
    }
    
    public static string GetLogPath()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return LogPath!;
    }
    
    public static string GetLogFile()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return LogFile!;
    }
    
    public static string GetDatabasePath()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return DatabasePath!;
    }
    
    public static string GetDatabaseFile()
    {
        if(!IsInitialized)
            throw new ArgumentNullException(nameof(AppName));
        return DatabaseFile!;
    }
    
    public static void Init(string appName)
    {
        if(string.IsNullOrEmpty(appName))
            throw new ArgumentNullException(nameof(appName));
        AppName = appName;
        Console.WriteLine($"{AppName} Configuration Service Init");
        BasePath = getBasePath();
        LogPath = Path.Combine(BasePath, "Logs");
        LogFile = Path.Combine(LogPath, "events.log");
        DatabasePath = Path.Combine(BasePath, "Data");
        DatabaseFile = Path.Combine(DatabasePath, $"{AppName}Db.sqlite");
        
        FileDirectoryHelper.CreateDirectory(BasePath);
        FileDirectoryHelper.CreateDirectory(DatabasePath);
        FileDirectoryHelper.CreateDirectory(LogPath);
        FileDirectoryHelper.CreateFile(LogFile);

        IsInitialized = true;
        Console.WriteLine($"{AppName} Configuration Service Init Complete");
    }
    
    private static string getBasePath()
    {
        return (Environment.OSVersion.Platform switch
        {
            // is application running on windows or linux
            PlatformID.Win32NT => 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), $"GitecSuite.{AppName}"),
            PlatformID.Unix => 
                Path.Combine("/var/lib/", $"GitecSuite.{AppName}"),
            _ => throw new PlatformNotSupportedException()
        })!;
    }
}

  