using System.Runtime.InteropServices;
using GitecEngine.ExceptionHandling;
using GitecEngine.Utilities;

namespace GitecEngine.Services;

public class ConfigurationService
{
    private bool _isInitialized;
    private string? AppName { get; set; }
    private string? BasePath { get; set; }
    private string? LogPath { get; set; }
    private string? LogFile { get; set; }
    private string? DatabasePath { get; set; }
    private string? DatabaseFile { get; set; }
    private string? TempPath { get; set; }
    private string? UploadPath { get; set; }
    private string? AssetsPath { get; set; }

    public ConfigurationService(bool force = false)
    {
        if (force)
        {
            _isInitialized = false; 
            FileDirectoryHelper.DeleteDirectory(GetBasePath());
        }
    }

    private void EnsureInitialized()
    {
        if (!_isInitialized)
            throw new ConfigurationException("Configuration is not initialized.");
    }

    private string EnsurePath(string? path, string errorMessage)
    {
        EnsureInitialized();
        return path ?? throw new ConfigurationException(errorMessage);
    }

    public string GetAssetsPath() =>  EnsurePath(AssetsPath, "Assets Path is not set.");
    public string GetUploadPath() => EnsurePath(UploadPath, "Upload Path is not set.");
    public string GetTempPath() => EnsurePath(TempPath, "Temp Path is not set.");
    public string GetAppName() => EnsurePath(AppName, "App Name is not set.");
    private string GetBasePath() => EnsurePath(BasePath, "Base Path is not set.");
    public string GetLogPath() => EnsurePath(LogPath, "Log Path is not set.");
    public string GetLogFile() => EnsurePath(LogFile, "Log File is not set.");
    public string GetDatabasePath() => EnsurePath(DatabasePath, "Database Path is not set.");
    public string GetDatabaseFile() => EnsurePath(DatabaseFile, "Database File is not set.");
    
    public string GetConnectionString() =>
        $"Data Source={EnsurePath(DatabaseFile, "Database Connection is not set.")}";

    public void Initialize(string appName)
    {
        if (string.IsNullOrWhiteSpace(appName))
            throw new ArgumentNullException(nameof(appName));

        try
        {
            AppName = appName;
            Console.WriteLine($"{AppName} Configuration Service Initializing...");

            BasePath = DetermineBasePath();
            LogPath = Path.Combine(BasePath, "Logs");
            LogFile = Path.Combine(LogPath, "events.log");
            DatabasePath = Path.Combine(BasePath, "Data");
            DatabaseFile = Path.Combine(DatabasePath, $"{AppName}Db.sqlite");
            TempPath = Path.Combine(BasePath, "Temp");
            UploadPath = Path.Combine(BasePath, "Uploads");
            AssetsPath = Path.Combine(BasePath, "Assets");

            CreateRequiredDirectories();
            _isInitialized = true;

            Console.WriteLine($"{AppName} Configuration Service Initialized.");
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Failed to initialize configuration service.", ex);
        }
    }

    private void CreateRequiredDirectories()
    {
        FileDirectoryHelper.CreateDirectory(BasePath!);
        FileDirectoryHelper.CreateDirectory(DatabasePath!);
        FileDirectoryHelper.CreateDirectory(LogPath!);
        FileDirectoryHelper.CreateFile(LogFile!);
        FileDirectoryHelper.CreateDirectory(TempPath!);
        FileDirectoryHelper.CreateDirectory(UploadPath!);
        FileDirectoryHelper.CreateDirectory(AssetsPath!);
    }

    private string DetermineBasePath()
    {
        try
        {
            string basePath = Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}"),
                
                PlatformID.Unix when RuntimeInformation.IsOSPlatform(OSPlatform.OSX) => Path.Combine(
                    "/usr/local/var", CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}"),
                
                PlatformID.Unix => Path.Combine(
                    "/var/lib", CoreConstants.CoreName, $"{CoreConstants.CoreName}.{AppName}"),
                
                _ => throw new PlatformNotSupportedException("The current operating system is not supported.")
            };

            FileDirectoryHelper.CreateDirectory(basePath);
            return basePath;
        }
        catch (Exception ex)
        {
            throw new ConfigurationException("Error while determining the base path.", ex);
        }
    }
}