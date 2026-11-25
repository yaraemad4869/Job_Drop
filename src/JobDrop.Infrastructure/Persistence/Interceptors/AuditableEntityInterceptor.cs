using JobDrop.Application.Common;
using JobDrop.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JobDrop.Infrastructure.Persistence.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTime;

    public AuditableEntityInterceptor(ICurrentUser currentUser, IDateTimeProvider dateTime)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var now = _dateTime.UtcNow;
        var actor = _currentUser.UserId?.ToString() ?? "system";

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = actor;
                entry.Entity.CreatedAtUtc = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = actor;
                entry.Entity.LastModifiedAtUtc = now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}