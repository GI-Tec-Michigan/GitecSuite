namespace GitecEngine.ExceptionHandling;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName)
        : base(entityName)
    {
        Console.WriteLine($"Entity not found: {entityName}");
    }
    
    public EntityNotFoundException(string entityName, string entityType)
        : base(entityName)
    {
        Console.WriteLine($"Entity not found: {entityName} [{entityType}]");
    }
}