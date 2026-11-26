using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Infrastructure.Persistence;

namespace JobDrop.Infrastructure.Notifications;

/// <summary>
/// Publishes events to the transactional outbox for reliable background processing.
/// </summary>
public sealed class OutboxPublisher : JobDrop.Application.Contracts.Persistence.IOutboxPublisher
{
    private readonly ApplicationDbContext _db;

    public OutboxPublisher(ApplicationDbContext db) => _db = db;

    public async Task PublishAsync(string eventType, object payload, CancellationToken cancellationToken = default)
    {
        // Use factory method to create OutboxMessage with proper encapsulation
        var outboxMessage = OutboxMessage.Create(
            eventType: eventType,
            payloadJson: System.Text.Json.JsonSerializer.Serialize(payload));

        _db.OutboxMessages.Add(outboxMessage);
        await _db.SaveChangesAsync(cancellationToken);
    }
}