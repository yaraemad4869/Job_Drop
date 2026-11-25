namespace JobDrop.Domain.Exceptions;

public sealed class NotFoundException : DomainException
{
    public string EntityName { get; }
    public object? KeyValue { get; }

    public NotFoundException(string entityName, object? keyValue)
        : base($"{entityName} with key '{keyValue}' was not found.", "RESOURCE_NOT_FOUND")
    {
        EntityName = entityName;
        KeyValue = keyValue;
    }

    public NotFoundException(string message)
        : base(message, "RESOURCE_NOT_FOUND")
    {
        EntityName = string.Empty;
    }
}