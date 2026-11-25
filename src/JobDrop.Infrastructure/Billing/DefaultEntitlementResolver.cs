using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Infrastructure.Billing;

public sealed class DefaultEntitlementResolver : IEntitlementResolver
{
    private readonly IApplicationDbContext _db;

    public DefaultEntitlementResolver(IApplicationDbContext db) => _db = db;

    public async Task<Entitlements> ResolveForCandidateAsync(Guid candidateProfileId, CancellationToken cancellationToken)
    {
        var profile = await _db.CandidateProfiles.AsNoTracking()
            .Where(p => p.Id == candidateProfileId)
            .Select(p => new { p.UserId })
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
            return new Entitlements(false, 5, 0, 10, 20_000_000, false, false);

        var isPro = await _db.Subscriptions.AsNoTracking()
            .AnyAsync(s => s.UserId == profile.UserId && s.Status == Domain.Enums.SubscriptionStatus.Active, cancellationToken);

        return isPro
            ? new Entitlements(true, 20, 0, 20, 50_000_000, false, false)
            : new Entitlements(false, 5, 0, 10, 20_000_000, false, false);
    }

    public async Task<Entitlements> ResolveForCompanyAsync(Guid companyId, CancellationToken cancellationToken)
    {
        var isPro = await _db.Subscriptions.AsNoTracking()
            .AnyAsync(s => s.CompanyId == companyId && s.Status == Domain.Enums.SubscriptionStatus.Active, cancellationToken);

        return isPro
            ? new Entitlements(true, 5, 30, 10, 20_000_000, true, true)
            : new Entitlements(false, 5, 0, 10, 20_000_000, false, false);
    }
}