namespace JobDrop.Domain.Common;

public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset OccurredAtUtc { get; init; } = DateTimeOffset.UtcNow;
}