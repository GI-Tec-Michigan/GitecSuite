using Gitec.ExceptionHandling;
using Gitec.Utilities;
using System.Runtime.InteropServices;

namespace Gitec.Services;

public class ConfigurationService
{
    private bool IsInitialized { get; set; }
    private string? AppName { get; set; }
    private string? BasePath { get; set; }
    private string? LogPath { get; set; }
    private string? LogFile { get; set; }
    private string? DatabasePath { get; set; }
    private string? DatabaseFile { get; set; }
    private string? TempPath { get; set; }
    private string? UploadPath { get; set; }
    private string? AssetsPath { get; set; }
    private string? ImagePath { get; set; }
    
    public string GetImagePath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Image Path is not set.");
        return ImagePath!;
    }

    private string GetAssetsPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Assets Path is not set.");
        return AssetsPath!;
    }

    private string GetUploadPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Upload Path is not set.");
        return UploadPath!;
    }
    public string GetTempPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. File Path is not set.");
        return TempPath!;
    }

    public string GetAppName()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. App Name is not set.");
        return AppName!;
    }

    public string GetBasePath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Base Path is not set.");
        return BasePath!;
    }

    public string GetLogPath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Log Path is not set.");
        return LogPath!;
    }

    public string GetLogFile()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Log File is not set.");
        return LogFile!;
    }

    public string GetDatabasePath()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Database Path is not set.");
        return DatabasePath!;
    }

    public string GetDatabaseFile()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. DatabaseFile is not set.");
        return DatabaseFile!;
    }

    public string GetConnectionString()
    {
        if (!IsInitialized)
            throw new ConfigurationException("Configuration is not initialized. Database Connection is not set.");
        return $"Data Source={DatabaseFile}";
    }

    public void Initialize(string appName)
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
            ImagePath = Path.Combine(UploadPath, "Images");
        
            FileDirectoryHelper.CreateDirectory(BasePath);
            FileDirectoryHelper.CreateDirectory(DatabasePath);
            FileDirectoryHelper.CreateDirectory(LogPath);
            FileDirectoryHelper.CreateFile(LogFile);
            FileDirectoryHelper.CreateDirectory(TempPath);
            FileDirectoryHelper.CreateDirectory(UploadPath);
            FileDirectoryHelper.CreateDirectory(AssetsPath);
            FileDirectoryHelper.CreateDirectory(ImagePath);

            IsInitialized = true;
            Console.WriteLine($"{AppName} Configuration Service Initialize Complete");
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Failed to initialize configuration service.", ex);
        }
    }

    private string GetBasePathInternal()
    {
        try
        {
            // Determine the base path based on the operating system.
            string basePath;
            switch (Environment.OSVersion.Platform)
            {
                // Windows: Use the common application data folder.
                case PlatformID.Win32NT:
                    basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}");
                    break;
                // Unix: Check if macOS or Linux.
                case PlatformID.Unix when RuntimeInformation.IsOSPlatform(OSPlatform.OSX):
                    basePath = Path.Combine("/usr/local/var", CoreConstants.CoreName,
                        $"{CoreConstants.CoreName}.{AppName}");
                    break;
                case PlatformID.Unix:
                    basePath = Path.Combine("/var/lib", CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}");
                    break;
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Xbox:
                case PlatformID.MacOSX:
                case PlatformID.Other:
                default:
                    // Unsupported platform.
                    throw new PlatformNotSupportedException("The current operating system is not supported.");
            }

            if (!Directory.Exists(basePath))
            {
                FileDirectoryHelper.CreateDirectory(basePath);
            }

            return basePath;
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Error while determining the base path.", ex);
        }
    }
}

  