namespace JobDrop.Domain.Services;

/// <summary>
/// Deterministic explainable matching engine.
/// CompositeScore = 0.35*semantic + 0.30*commute + 0.20*experience + 0.15*compensation
/// Anti-bias: Age, Gender, Photo, Nationality, Family Name excluded.
/// </summary>
public sealed class MatchScoringEngine
{
    private const double WeightSemantic = 0.35;
    private const double WeightCommute = 0.30;
    private const double WeightExperience = 0.20;
    private const double WeightCompensation = 0.15;

    public int CalculateMatchScore(
        double semanticSimilarity,
        double commuteEfficiency,
        double experienceMatch,
        double compensationOverlap)
    {
        var score = (WeightSemantic * Clamp01(semanticSimilarity))
                  + (WeightCommute * Clamp01(commuteEfficiency))
                  + (WeightExperience * Clamp01(experienceMatch))
                  + (WeightCompensation * Clamp01(compensationOverlap));

        return (int)Math.Round(100 * Clamp01(score));
    }

    /// <summary>
    /// Calculates commute efficiency: max(0, 1 - duration/maxDuration)
    /// </summary>
    public double CalculateCommuteEfficiency(int durationSeconds, int maxDurationSeconds)
    {
        if (maxDurationSeconds <= 0) return 0;
        var ratio = (double)durationSeconds / maxDurationSeconds;
        return Math.Max(0, 1 - ratio);
    }

    /// <summary>
    /// Calculates skill match ratio.
    /// </summary>
    public double CalculateSkillMatch(
        IReadOnlyCollection<string> requiredSkills,
        IReadOnlyCollection<string> candidateSkills)
    {
        if (requiredSkills.Count == 0) return 1.0;
        var matched = requiredSkills.Count(s => candidateSkills.Contains(s, StringComparer.OrdinalIgnoreCase));
        return (double)matched / requiredSkills.Count;
    }

    /// <summary>
    /// Calculates compensation overlap.
    /// </summary>
    public double CalculateCompensationOverlap(
        int? jobMin, int? jobMax,
        int? candidateMin, int? candidateMax)
    {
        if (!jobMin.HasValue || !jobMax.HasValue) return 0.5; // Undisclosed salary
        if (!candidateMin.HasValue || !candidateMax.HasValue) return 0.5;

        var overlap = Math.Min(jobMax.Value, candidateMax.Value) - Math.Max(jobMin.Value, candidateMin.Value);
        if (overlap <= 0) return 0;

        var range = jobMax.Value - jobMin.Value;
        return range > 0 ? Math.Min(1.0, (double)overlap / range) : 0;
    }

    private static double Clamp01(double value) => Math.Clamp(value, 0, 1);
}