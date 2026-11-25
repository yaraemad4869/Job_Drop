using JobDrop.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JobDrop.Infrastructure.Persistence.Interceptors;

public sealed class DomainEventsDispatcherInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventsDispatcherInterceptor(IMediator mediator) => _mediator = mediator;

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

        var entities = eventData.Context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}