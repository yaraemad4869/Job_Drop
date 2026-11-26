using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Drops;

public sealed class ApplicationSla : BaseEntity
{
    public Guid ApplicationId { get; private set; }
    public DateTime StartedAtUtc { get; private set; }
    public DateTime DueAtUtc { get; private set; }
    public DateTime? RespondedAtUtc { get; private set; }
    public SlaStatus Status { get; private set; }
    
    // Navigation Property
    public Application? Application { get; private set; }

    private ApplicationSla() { }

    public ApplicationSla(Guid applicationId, DateTime startedAtUtc, int slaHours = 48)
    {
        ApplicationId = applicationId;
        StartedAtUtc = startedAtUtc;
        DueAtUtc = startedAtUtc.AddHours(slaHours);
        Status = SlaStatus.Pending;
    }

    public void MarkResponded()
    {
        RespondedAtUtc = DateTime.UtcNow;
        Status = SlaStatus.Responded;
    }

    public void MarkExpiredRefunded()
    {
        Status = SlaStatus.ExpiredRefunded;
    }
    public byte[] RowVersion { get; private set; } = null!;
}

