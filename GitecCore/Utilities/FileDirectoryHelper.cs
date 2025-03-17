using System.Collections.ObjectModel;

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
        Console.WriteLine($"Wrote File: {path}");
    }

    public static void AppendFile(string path, string content)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        File.AppendAllText(path, content);
        Console.WriteLine($"Appended to File: {path}");
    }
    
    public static void DeleteFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        File.Delete(path);
        Console.WriteLine($"Deleted File: {path}");
    }
    
    public static void MoveFile(string sourcePath, string destPath)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);
        File.Move(sourcePath, destPath);
        Console.WriteLine($"Moved File from {sourcePath} to {destPath}");
    }
    
    public static void CopyFile(string sourcePath, string destPath)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException(sourcePath);
        File.Copy(sourcePath, destPath);
        Console.WriteLine($"Copied File from {sourcePath} to {destPath}");
    }
    
    public static void CreateDirectory(string path)
    {
        if (Directory.Exists(path))
            return;
        Directory.CreateDirectory(path);
        Console.WriteLine($"Created directory: {path}");
    }
    
    public static void DeleteDirectory(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);
        Directory.Delete(path, true);
        Console.WriteLine($"Deleted directory: {path}");
    }
    
    public static void MoveDirectory(string sourcePath, string destPath)
    {
        if (!Directory.Exists(sourcePath))
            throw new DirectoryNotFoundException(sourcePath);
        Directory.Move(sourcePath, destPath);
        Console.WriteLine($"Moved directory from {sourcePath} to {destPath}");
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
        Console.WriteLine($"Copied directory from {sourcePath} to {destPath}");
    }

    public static void CreateFile(string file, string content)
    {
        if (File.Exists(file))
            return;
        File.WriteAllText(file, content);
        Console.WriteLine($"Created file: {file}");
    }

    public static void CreateFile(string file)
    {
        if (File.Exists(file))
            return;
        File.Create(file);
        Console.WriteLine($"Created file: {file}");
    }
}