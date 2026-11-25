using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Companies;

public sealed class ReviewReport : BaseEntity
{
    public Guid ReviewId { get; set; }
    public Guid ReporterUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTimeOffset ReportedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string Status { get; set; } = "Pending";
}