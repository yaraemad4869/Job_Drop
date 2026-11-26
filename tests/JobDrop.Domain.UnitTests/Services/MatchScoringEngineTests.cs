using JobDrop.Domain.Services;
using Xunit;

namespace JobDrop.Domain.UnitTests.Services;

public sealed class MatchScoringEngineTests
{
    [Fact]
    public void CalculateMatchScore_AllPerfect_Returns100()
    {
        var engine = new MatchScoringEngine();

        var score = engine.CalculateMatchScore(
            semanticSimilarity: 1.0,
            commuteEfficiency: 1.0,
            experienceMatch: 1.0,
            compensationOverlap: 1.0);

        Assert.Equal(100, score);
    }

    [Fact]
    public void CalculateMatchScore_AllZero_Returns0()
    {
        var engine = new MatchScoringEngine();

        var score = engine.CalculateMatchScore(
            semanticSimilarity: 0.0,
            commuteEfficiency: 0.0,
            experienceMatch: 0.0,
            compensationOverlap: 0.0);

        Assert.Equal(0, score);
    }

    [Fact]
    public void CalculateMatchScore_WeightsCorrectly()
    {
        var engine = new MatchScoringEngine();

        // 0.35 * 1.0 + 0.30 * 0.5 + 0.20 * 0.5 + 0.15 * 0.5 = 0.35 + 0.15 + 0.10 + 0.075 = 0.675
        var score = engine.CalculateMatchScore(
            semanticSimilarity: 1.0,
            commuteEfficiency: 0.5,
            experienceMatch: 0.5,
            compensationOverlap: 0.5);

        Assert.Equal(68, score); // 0.675 * 100 = 67.5 -> rounds to 68
    }

    [Fact]
    public void CalculateCommuteEfficiency_AtMaxDuration_Returns0()
    {
        var engine = new MatchScoringEngine();

        var efficiency = engine.CalculateCommuteEfficiency(
            durationSeconds: 3600,
            maxDurationSeconds: 3600);

        Assert.Equal(0.0, efficiency);
    }

    [Fact]
    public void CalculateCommuteEfficiency_AtZeroDuration_Returns1()
    {
        var engine = new MatchScoringEngine();

        var efficiency = engine.CalculateCommuteEfficiency(
            durationSeconds: 0,
            maxDurationSeconds: 3600);

        Assert.Equal(1.0, efficiency);
    }

    [Fact]
    public void CalculateSkillMatch_AllMatched_Returns1()
    {
        var engine = new MatchScoringEngine();
        var required = new[] { "C#", ".NET", "SQL" };
        var candidate = new[] { "C#", ".NET", "SQL", "JavaScript" };

        var match = engine.CalculateSkillMatch(required, candidate);

        Assert.Equal(1.0, match);
    }

    [Fact]
    public void CalculateSkillMatch_NoneMatched_Returns0()
    {
        var engine = new MatchScoringEngine();
        var required = new[] { "C#", ".NET", "SQL" };
        var candidate = new[] { "JavaScript", "Python" };

        var match = engine.CalculateSkillMatch(required, candidate);

        Assert.Equal(0.0, match);
    }
}