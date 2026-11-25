using System.Text.Json;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Infrastructure.Persistence;

namespace JobDrop.Infrastructure.Notifications;

public sealed class OutboxPublisher : IOutboxPublisher
{
    private readonly ApplicationDbContext _db;

    public OutboxPublisher(ApplicationDbContext db) => _db = db;

    public async Task PublishAsync(string eventType, object payload, CancellationToken cancellationToken = default)
    {
        var msg = new OutboxMessage
        {
            EventType = eventType,
            PayloadJson = JsonSerializer.Serialize(payload),
            NextAttemptAtUtc = DateTimeOffset.UtcNow
        };
        _db.Set<OutboxMessage>().Add(msg);
        await _db.SaveChangesAsync(cancellationToken);
    }
}