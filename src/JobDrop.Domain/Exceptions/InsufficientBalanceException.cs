namespace JobDrop.Domain.Exceptions;

public sealed class InsufficientBalanceException : DomainException
{
    public InsufficientBalanceException(string message = "Insufficient wallet balance.")
        : base(message, "INSUFFICIENT_BALANCE")
    {
    }
}