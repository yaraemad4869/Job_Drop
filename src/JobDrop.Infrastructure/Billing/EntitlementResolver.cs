using JobDrop.Application.Contracts.Billing;

namespace JobDrop.Infrastructure.Billing;

public sealed class EntitlementResolver : IEntitlementResolver
{
    public Task<Entitlements> ResolveForCandidateAsync(Guid candidateProfileId, CancellationToken ct)
    {
        // TODO: Resolve from Subscription table
        return Task.FromResult(new Entitlements(
            IsPro: false,
            DailyDropLimit: 5,
            MonthlyReverseDropPool: 0,
            MaxDocuments: 10,
            MaxTotalDocumentBytes: 20L * 1024 * 1024,
            CanUseTalentSearch: false,
            CanTransferSpecialDrops: false
        ));
    }

    public Task<Entitlements> ResolveForCompanyAsync(Guid companyId, CancellationToken ct)
    {
        // TODO: Resolve from Subscription table
        return Task.FromResult(new Entitlements(
            IsPro: false,
            DailyDropLimit: 20,
            MonthlyReverseDropPool: 30,
            MaxDocuments: 10,
            MaxTotalDocumentBytes: 20L * 1024 * 1024,
            CanUseTalentSearch: false,
            CanTransferSpecialDrops: false
        ));
    }
}