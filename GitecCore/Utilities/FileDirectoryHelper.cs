namespace Gitec.Utilities;

public static class FileDirectoryHelper
{
    public static string ReadFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        return File.ReadAllText(path);
    }
    
    public static void WriteFile(string path, string content, bool overwrite = false)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        if (!overwrite)
            throw new IOException($"File already exists: {path}");
        File.WriteAllText(path, content);
    }

    public static void AppendFile(string path, string content)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        File.AppendAllText(path, content);
    }
    
    public static void DeleteFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        File.Delete(path);
    }
    
    public static void MoveFile(string sourcePath, string destPath)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);
        File.Move(sourcePath, destPath);
    }
    
    public static void CopyFile(string sourcePath, string destPath)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);
        File.Copy(sourcePath, destPath);
    }
    
    public static void CreateDirectory(string path)
    {
        if (Directory.Exists(path))
            return;
        Directory.CreateDirectory(path);
    }
    
    public static void DeleteDirectory(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);
        Directory.Delete(path, true);
    }
    
    public static void MoveDirectory(string sourcePath, string destPath)
    {
        if (!Directory.Exists(sourcePath))
            throw new DirectoryNotFoundException(sourcePath);
        Directory.Move(sourcePath, destPath);
    }
    
    public static void CopyDirectory(string sourcePath, string destPath)
    {
        if (!Directory.Exists(sourcePath))
            throw new DirectoryNotFoundException(sourcePath);
        Directory.CreateDirectory(destPath);
        foreach (var file in Directory.GetFiles(sourcePath))
        {
            var destFile = Path.Combine(destPath, Path.GetFileName(file));
            File.Copy(file, destFile);
        }
        foreach (var directory in Directory.GetDirectories(sourcePath))
        {
            var destDirectory = Path.Combine(destPath, Path.GetFileName(directory));
            CopyDirectory(directory, destDirectory);
        }
    }

    public static void CreateFile(string file, string content)
    {
        if (File.Exists(file))
            return;
        File.WriteAllText(file, content);
    }

    public static void CreateFile(string file)
    {
        if (File.Exists(file))
            return;
        File.Create(file);
    }
}