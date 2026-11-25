using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyOffice : ConcurrentAuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid OperatingAreaId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public OfficeType OfficeType { get; set; }
    public GeoPoint Location { get; set; } = new GeoPoint(0, 0);
    public int GeofenceRadiusMeters { get; set; } = 500;
    public string? TransitAnchorText { get; set; }
    public string? MakaniCode { get; set; }
    public string? PlusCode { get; set; }
    public VerificationLevel VerificationLevel { get; set; } = VerificationLevel.Unverified;
    public int LocationVersion { get; set; } = 1;

    public Company? Company { get; set; }
    public Geography.OperatingArea? OperatingArea { get; set; }
}