namespace JobDrop.Application.Contracts.Persistence;

public interface IOutboxPublisher
{
    Task PublishAsync(string eventType, object payload, CancellationToken cancellationToken = default);
}