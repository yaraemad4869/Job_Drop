using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Entities.Jobs;

namespace JobDrop.Domain.Entities.Companies;

public sealed class Company : ConcurrentAuditableEntity
{
    public string LegalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? ThemeColorHex { get; set; }
    public string? CommercialLicenseNo { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public string? LogoStorageKey { get; set; }
    public string Industry { get; set; } = string.Empty;
    public int SizeBand { get; set; }
    public VerificationLevel VerificationLevel { get; set; } = VerificationLevel.Unverified;
    public bool IsActive { get; set; } = true;

    public ICollection<CompanyOffice> Offices { get; set; } = new List<CompanyOffice>();
    public ICollection<CompanyMember> Members { get; set; } = new List<CompanyMember>();
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}