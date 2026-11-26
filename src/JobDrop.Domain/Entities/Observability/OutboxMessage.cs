using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Observability;

public sealed class OutboxMessage : BaseEntity
{
    public string EventType { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = "{}";
    public int RetryCount { get; set; }
    public DateTimeOffset NextAttemptAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAtUtc { get; set; }
    public DateTimeOffset? DeadLetteredAtUtc { get; set; }
    public string? LastError { get; set; }

    public JobDrop.Domain.Enums.OutboxStatus Status { get; private set; } = JobDrop.Domain.Enums.OutboxStatus.Pending;

    public void MarkDelivered()
    {
        Status = JobDrop.Domain.Enums.OutboxStatus.Delivered;
    }

    public void MarkFailed(string? errorMessage = null)
    {
        Status = JobDrop.Domain.Enums.OutboxStatus.Failed;
        RetryCount++;
        NextAttemptAtUtc = DateTime.UtcNow.AddMinutes(Math.Pow(2, RetryCount));
    }

    public void MarkDeadLettered()
    {
        Status = JobDrop.Domain.Enums.OutboxStatus.DeadLettered;
        DeadLetteredAtUtc = DateTime.UtcNow;
    }

    public JobDrop.Domain.Enums.NotificationChannel Channel { get; private set; } = JobDrop.Domain.Enums.NotificationChannel.InApp;
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Factory method to create a new OutboxMessage with pending status.
    /// </summary>
    public static OutboxMessage Create(string eventType, string payloadJson)
    {
        var message = new OutboxMessage();
        message.EventType = eventType;
        message.PayloadJson = payloadJson;
        message.Status = JobDrop.Domain.Enums.OutboxStatus.Pending;
        message.RetryCount = 0;
        message.NextAttemptAtUtc = System.DateTimeOffset.UtcNow;
        return message;
    }
}
