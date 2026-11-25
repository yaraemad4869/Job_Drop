using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Drops;

public sealed class ApplicationEvent : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public DropStatus FromStatus { get; set; }
    public DropStatus ToStatus { get; set; }
    public string? Comment { get; set; }
    public Guid ActorUserId { get; set; }
    public ActorType ActorType { get; set; }
    public DateTimeOffset OccurredAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public Application? Application { get; set; }
}