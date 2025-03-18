namespace GitecEngine.ExceptionHandling;

public class UnknownTypeException : Exception
{
    public UnknownTypeException(string typeName)
        : base(typeName)
    {
        Console.WriteLine($"Unknown type: {typeName}");
    }
    
    public UnknownTypeException(string typeName, string typeNamespace)
        : base(typeName)
    {
        Console.WriteLine($"Unknown type: {typeName} [{typeNamespace}]");
    }

}