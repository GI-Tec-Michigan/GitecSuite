namespace GitecEngine.ExceptionHandling;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message)
        : base(message)
    {
        Console.WriteLine($"Configuration Exception: {message}");
    }

    public ConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Console.WriteLine($"Configuration Exception: {message}, Inner Exception: {innerException.Message}");
    }
}