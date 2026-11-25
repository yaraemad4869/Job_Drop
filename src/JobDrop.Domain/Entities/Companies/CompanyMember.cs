using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Entities.Identity;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyMember : BaseEntity
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public CompanyMemberRole Role { get; set; }
    public string? DepartmentScope { get; set; }
    public DateTimeOffset JoinedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;

    public Company? Company { get; set; }
    public AppUser? User { get; set; }
}