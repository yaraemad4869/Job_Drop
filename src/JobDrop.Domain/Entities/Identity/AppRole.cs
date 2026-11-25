using Microsoft.AspNetCore.Identity;

namespace JobDrop.Domain.Entities.Identity;

public sealed class AppRole : IdentityRole<Guid>
{
    public const string JobSeeker = "JobSeeker";
    public const string CompanyOwner = "CompanyOwner";
    public const string CompanyMember = "CompanyMember";
    public const string PlatformAdmin = "PlatformAdmin";
}