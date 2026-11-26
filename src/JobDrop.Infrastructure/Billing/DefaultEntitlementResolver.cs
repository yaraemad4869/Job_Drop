using JobDrop.Application.Contracts.Billing;

namespace JobDrop.Infrastructure.Billing;

/// <summary>
/// Resolves user entitlements based on subscription plan.
/// Single source of truth for Premium quotas across all features.
/// </summary>
public sealed class DefaultEntitlementResolver : IEntitlementResolver
{
    public Task<Entitlements> ResolveAsync(Guid userId, CancellationToken ct)
    {
        // TODO: Query Subscription + Plan tables for actual user entitlements.
        // For now, return Free tier defaults.
        return Task.FromResult(new Entitlements(
            PlanCode: "Free",
            DailyDropLimit: 5,
            BonusDropLimit: 0,
            SpecialDropLimit: 0,
            ReverseDropMonthlyPool: 0,
            CanAccessTalentSearch: false,
            MaxDocuments: 10,
            MaxTotalStorageBytes: 20L * 1024 * 1024));
    }
}