namespace JobDrop.Application.Common;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Email { get; }
    string? Handle { get; }
    bool IsAuthenticated { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role);
}