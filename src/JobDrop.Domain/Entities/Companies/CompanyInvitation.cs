using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyInvitation : BaseEntity
{
    public Guid CompanyId { get; set; }
    public string Email { get; set; } = string.Empty;
    public CompanyMemberRole Role { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? AcceptedAtUtc { get; set; }
    public Guid? InvitedByUserId { get; set; }
}