using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.CompanyRankings;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Services;

/// <summary>
/// BE-14: Calculates company rankings with ordinal rank and eligible count.
/// Prevents misleading positions for companies below eligibility threshold.
/// </summary>
public class CompanyRankingService
{
    private readonly IApplicationDbContext _context;

    public CompanyRankingService(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets ranking for a company in a specific scope.
    /// BE-14: Returns ordinal rank with eligible count, no misleading positions.
    /// </summary>
    public async Task<CompanyRankingDto?> GetRankingAsync(
        Guid companyId,
        string scope,
        CancellationToken cancellationToken = default)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);

        if (company == null)
        {
            return null;
        }

        // BE-14: Check if eligible for display
        if (!company.EligibleCompanyCount.HasValue || company.EligibleCompanyCount.Value < 10)
        {
            return null; // BE-14: Do not display misleading positions
        }

        return new CompanyRankingDto(
            CompanyId: company.Id,
            OrdinalRank: company.OrdinalRank ?? 0,
            EligibleCompanyCount: company.EligibleCompanyCount ?? 0,
            Percentile: company.Percentile,
            FollowUpRate: company.FollowUpRate
        );
    }

    /// <summary>
    /// Calculates ordinal rank based on follow-up rate.
    /// </summary>
    public int CalculateOrdinalRank(double followUpRate, List<double> allRates)
    {
        var sorted = allRates.OrderByDescending(r => r).ToList();
        return sorted.IndexOf(followUpRate) + 1;
    }

    /// <summary>
    /// Checks if company is eligible for display.
    /// BE-14: Minimum sample size required.
    /// </summary>
    public bool IsEligibleForDisplay(int ordinalRank, int eligibleCount, int minSampleSize = 10)
    {
        return eligibleCount >= minSampleSize;
    }

    /// <summary>
    /// Calculates percentile.
    /// </summary>
    public double? CalculatePercentile(int ordinalRank, int eligibleCount)
    {
        if (eligibleCount == 0) return null;
        return 100.0 * (eligibleCount - ordinalRank + 1) / eligibleCount;
    }
}
