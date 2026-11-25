using System.ComponentModel.DataAnnotations.Schema;
using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Sla;

public sealed class ApplicationSla : ConcurrentAuditableEntity
{
    public Guid ApplicationId { get; set; }
    public DateTimeOffset StartedAtUtc { get; set; }
    public DateTimeOffset DueAtUtc { get; set; }
    public DateTimeOffset? RespondedAtUtc { get; set; }
    public SlaStatus Status { get; set; } = SlaStatus.Pending;
    public string PolicyVersion { get; set; } = "v1";
    public string CalendarSnapshotJson { get; set; } = "{}";

    public Drops.Application? Application { get; set; }

    [NotMapped]
    public bool IsLate => Status == SlaStatus.Pending && DateTimeOffset.UtcNow > DueAtUtc;
}