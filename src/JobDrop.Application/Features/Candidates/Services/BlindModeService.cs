using JobDrop.Domain.Entities.Candidates;

namespace JobDrop.Application.Features.Candidates.Services;

/// <summary>
/// BE-03 & BE-04: Enforces blind screening mode.
/// Returns CandidateCode only, never names or gender-derived avatars.
/// </summary>
public class BlindModeService
{
    /// <summary>
    /// Generates a stable, anonymous candidate code.
    /// </summary>
    public string GenerateCandidateCode(Guid candidateId, Guid companyId)
    {
        var hash = (candidateId.GetHashCode() ^ companyId.GetHashCode()).ToString("X8");
        return $"C-{hash.Substring(0, 8).ToUpperInvariant()}";
    }

    /// <summary>
    /// Returns a blind-safe DTO without PII.
    /// Note: MatchScore, MatchBand, Skills, and ExperienceYears are computed/provided
    /// by the calling context (e.g., search results, dossier projection) and are not
    /// stored directly on CandidateProfile. They should be passed as parameters.
    /// </summary>
    public BlindCandidateDto ToBlindDto(
        CandidateProfile profile,
        Guid companyId,
        double? matchScore = null,
        string? matchBand = null,
        IEnumerable<string>? skills = null,
        int? experienceYears = null)
    {
        return new BlindCandidateDto
        {
            CandidateCode = GenerateCandidateCode(profile.Id, companyId),
            MatchScore = matchScore,
            MatchBand = matchBand,
            Skills = skills?.ToList() ?? new List<string>(),
            ExperienceYears = experienceYears,
            AvatarUrl = "/assets/avatars/neutral.svg"
        };
    }
}

public class BlindCandidateDto
{
    public string CandidateCode { get; set; } = string.Empty;
    public double? MatchScore { get; set; }
    public string? MatchBand { get; set; }
    public List<string> Skills { get; set; } = new();
    public int? ExperienceYears { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
}
