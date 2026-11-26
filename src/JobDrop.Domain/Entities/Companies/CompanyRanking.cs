namespace JobDrop.Domain.Entities.Companies;

/// <summary>
/// BE-14: Company ranking with ordinal rank and eligible count per scope.
/// </summary>
public class CompanyRanking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public string Scope { get; set; } = string.Empty; // City, Country, Global
    public string ScopeKey { get; set; } = string.Empty; // e.g., "Cairo", "Egypt", "World"
    public int OrdinalRank { get; set; }
    public int EligibleCompanyCount { get; set; }
    public double? Percentile { get; set; }
    public double FollowUpRate { get; set; }
    public int SampleSize { get; set; }
    public DateTimeOffset CalculatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
