namespace JobDrop.Domain.Exceptions;

public sealed class CapacityExceededException : DomainException
{
    public CapacityExceededException(string message = "Capacity limit exceeded.")
        : base(message, "CAPACITY_EXCEEDED")
    {
    }
}