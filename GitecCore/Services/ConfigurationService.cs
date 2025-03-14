using Gitec.ExceptionHandling;
using Gitec.Utilities;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

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
    
    private static string? TempPath { get; set; }
    private static string? UploadPath { get; set; }
    private static string? AssetsPath { get; set; }

    private static string? GetAssetsPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. AssetsPath is not set.");
        return AssetsPath!;
    }

    private static string? GetUploadPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. UploadPath is not set.");
        return UploadPath!;
    }
    public static string GetTempPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. FilePath is not set.");
        return TempPath!;
    }

    public static string GetAppName()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. AppName is not set.");
        return AppName!;
    }

    public static string GetBasePath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. BasePath is not set.");
        return BasePath!;
    }

    public static string GetLogPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. LogPath is not set.");
        return LogPath!;
    }

    public static string GetLogFile()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. LogFile is not set.");
        return LogFile!;
    }

    public static string GetDatabasePath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. DatabasePath is not set.");
        return DatabasePath!;
    }

    public static string GetDatabaseFile()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. DatabaseFile is not set.");
        return DatabaseFile!;
    }

    public static string GetConnectionString()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. DatabaseFile is not set.");
        return $"Data Source={DatabaseFile}";
    }

    public static void Initialize(string appName)
    {
        try { 

            if(string.IsNullOrEmpty(appName))
                throw new ArgumentNullException(nameof(appName));
            AppName = appName;
            Console.WriteLine($"{AppName} Configuration Service Initialize");
            BasePath = GetBasePathInternal();
            LogPath = Path.Combine(BasePath, "Logs");
            LogFile = Path.Combine(LogPath, "events.log");
            DatabasePath = Path.Combine(BasePath, "Data");
            DatabaseFile = Path.Combine(DatabasePath, $"{AppName}Db.sqlite");
            TempPath = Path.Combine(BasePath, "Temp");
            UploadPath = Path.Combine(BasePath, "Uploads");
            AssetsPath = Path.Combine(BasePath, "Assets");
        
            FileDirectoryHelper.CreateDirectory(BasePath);
            FileDirectoryHelper.CreateDirectory(DatabasePath);
            FileDirectoryHelper.CreateDirectory(LogPath);
            FileDirectoryHelper.CreateFile(LogFile);
            FileDirectoryHelper.CreateDirectory(TempPath);
            FileDirectoryHelper.CreateDirectory(UploadPath);
            FileDirectoryHelper.CreateDirectory(AssetsPath);

            IsInitialized = true;
            Console.WriteLine($"{AppName} Configuration Service Initialize Complete");
        }
        catch (Exception ex)
        {
            // Optionally log the exception here before rethrowing.
            throw new ConfigurationException("Failed to initialize configuration service.", ex);
        }
    }

    private static string GetBasePathInternal()
    {
        try
        {
            // Determine the base path based on the operating system.
            string basePath = Environment.OSVersion.Platform switch
            {
                // Windows: Use the common application data folder.
                PlatformID.Win32NT => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    CoreConstants.CoreName,
                    $"{CoreConstants.CoreName}.{AppName}"
                ),

                // Unix: Check if macOS or Linux.
                PlatformID.Unix when RuntimeInformation.IsOSPlatform(OSPlatform.OSX) =>
                    Path.Combine("/usr/local/var", CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}"),
                PlatformID.Unix =>
                    Path.Combine("/var/lib", CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}"),

                // Unsupported platform.
                _ => throw new PlatformNotSupportedException("The current operating system is not supported.")
            };

            // Create the directory if it doesn't exist.
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return basePath;
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Error while determining the base path.", ex);
        }
    }
}

  