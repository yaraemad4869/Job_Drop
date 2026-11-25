namespace JobDrop.Domain.Exceptions;

public sealed class ForbiddenAccessException : DomainException
{
    public ForbiddenAccessException(string message = "You do not have permission to perform this action.")
        : base(message, "FORBIDDEN")
    {
    }
}