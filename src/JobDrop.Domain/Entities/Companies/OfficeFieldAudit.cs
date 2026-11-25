using JobDrop.Domain.Common;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.Entities.Companies;

public sealed class OfficeFieldAudit : BaseEntity
{
    public Guid CompanyOfficeId { get; set; }
    public string AuditHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? ScannedAtUtc { get; set; }
    public GeoPoint? GpsPoint { get; set; }
    public string? FieldEvidenceKey { get; set; }
    public string Status { get; set; } = "Pending";
    public Guid? ReviewedByAdminId { get; set; }
    public DateTimeOffset? ReviewedAtUtc { get; set; }
}