namespace JobDrop.Application.Contracts.Billing;

/// <summary>
/// Single source of truth for Premium entitlements across all quotas.
/// </summary>
public interface IEntitlementResolver
{
    Task<Entitlements> ResolveAsync(Guid userId, CancellationToken ct);
}

public sealed record Entitlements(
    string PlanCode,
    int DailyDropLimit,
    int BonusDropLimit,
    int SpecialDropLimit,
    int ReverseDropMonthlyPool,
    bool CanAccessTalentSearch,
    int MaxDocuments,
    long MaxTotalStorageBytes);