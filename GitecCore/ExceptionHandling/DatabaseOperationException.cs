using System;

namespace InfoDisplay.Data;

/// <summary>
/// Custom exception class for handling database operation errors.
/// </summary>
public class DatabaseOperationException : Exception
{
    public DatabaseOperationException(string message)
        : base(message)
    {
    }

    public DatabaseOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
