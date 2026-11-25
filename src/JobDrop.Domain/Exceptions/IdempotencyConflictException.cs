namespace JobDrop.Domain.Exceptions;

public sealed class IdempotencyConflictException : DomainException
{
    public string IdempotencyKey { get; }

    public IdempotencyConflictException(string idempotencyKey)
        : base($"An operation with idempotency key '{idempotencyKey}' has already been processed.", "IDEMPOTENCY_CONFLICT")
    {
        IdempotencyKey = idempotencyKey;
    }
}