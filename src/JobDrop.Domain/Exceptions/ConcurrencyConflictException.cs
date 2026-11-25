namespace JobDrop.Domain.Exceptions;

public sealed class ConcurrencyConflictException : DomainException
{
    public string EntityName { get; }

    public ConcurrencyConflictException(string entityName)
        : base($"Concurrency conflict on {entityName}. The record was modified by another user.", "CONCURRENCY_CONFLICT")
    {
        EntityName = entityName;
    }
}