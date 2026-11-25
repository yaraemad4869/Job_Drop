namespace JobDrop.Application.Contracts.Billing;

public sealed record Entitlements(
    bool IsPro,
    int DailyDropLimit,
    int MonthlyReverseDropPool,
    int MaxDocuments,
    long MaxTotalDocumentBytes,
    bool CanUseTalentSearch,
    bool CanTransferSpecialDrops);

public interface IEntitlementResolver
{
    Task<Entitlements> ResolveForCandidateAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
    Task<Entitlements> ResolveForCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
}