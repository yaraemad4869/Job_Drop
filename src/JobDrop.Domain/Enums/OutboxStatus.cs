namespace JobDrop.Domain.Enums;

/// <summary>
/// Outbox message processing states.
/// </summary>
public enum OutboxStatus
{
    Pending = 0,
    Processing = 1,
    Delivered = 2,
    Failed = 3,
    DeadLettered = 4
}