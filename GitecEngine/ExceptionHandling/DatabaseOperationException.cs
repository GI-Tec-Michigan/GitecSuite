namespace GitecEngine.ExceptionHandling;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException(string message)
        : base(message)
    {
        Console.WriteLine("Database Operation Exception: " + message);
    }
    public DatabaseOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Console.WriteLine("Database Operation Exception: " + message, innerException);
    }
}