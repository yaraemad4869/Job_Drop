namespace JobDrop.Domain.Services;

public sealed class MatchScoringEngine
{
    public const decimal WeightSemantic = 0.35m;
    public const decimal WeightCommute = 0.30m;
    public const decimal WeightExperience = 0.20m;
    public const decimal WeightCompensation = 0.15m;

    public decimal ComputeCompositeScore(
        decimal semanticScore,
        int commuteDurationSeconds,
        int maxCommuteSeconds,
        decimal experienceScore,
        decimal compensationScore)
    {
        var commute = maxCommuteSeconds > 0
            ? Math.Max(0m, 1m - (decimal)commuteDurationSeconds / maxCommuteSeconds)
            : 1m;

        var raw = (WeightSemantic * Clamp01(semanticScore))
                + (WeightCommute * commute)
                + (WeightExperience * Clamp01(experienceScore))
                + (WeightCompensation * Clamp01(compensationScore));

        return Math.Round(100m * raw, MidpointRounding.AwayFromZero);
    }

    private static decimal Clamp01(decimal v) => v < 0 ? 0 : v > 1 ? 1 : v;
}