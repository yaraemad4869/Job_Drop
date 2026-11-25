using Xunit;
using FluentAssertions;
using JobDrop.Domain.Services;

namespace JobDrop.Domain.UnitTests.Services;

public class MatchScoringEngineTests
{
    private readonly MatchScoringEngine _engine = new();

    [Fact]
    public void ComputeCompositeScore_AllPerfect_ShouldReturn100()
    {
        var score = _engine.ComputeCompositeScore(1.0m, 0, 3600, 1.0m, 1.0m);
        score.Should().Be(100);
    }

    [Fact]
    public void ComputeCompositeScore_AllZero_ShouldReturn0()
    {
        var score = _engine.ComputeCompositeScore(0.0m, 3600, 3600, 0.0m, 0.0m);
        score.Should().Be(0);
    }

    [Fact]
    public void ComputeCompositeScore_PartialScores_ShouldCalculateWeightedAverage()
    {
        var score = _engine.ComputeCompositeScore(0.8m, 1800, 3600, 0.6m, 0.9m);
        score.Should().BeGreaterThan(0);
        score.Should().BeLessThan(100);
    }

    [Fact]
    public void ComputeCompositeScore_CommuteExceedsMax_ShouldClampToZero()
    {
        var score = _engine.ComputeCompositeScore(1.0m, 7200, 3600, 1.0m, 1.0m);
        score.Should().BeLessThan(100);
    }

    [Fact]
    public void ComputeCompositeScore_NegativeScores_ShouldClampToZero()
    {
        var score = _engine.ComputeCompositeScore(-0.5m, 0, 3600, -0.3m, -0.2m);
        score.Should().Be(30);
    }

    [Fact]
    public void ComputeCompositeScore_ZeroMaxCommute_ShouldTreatAsPerfect()
    {
        var score = _engine.ComputeCompositeScore(1.0m, 1000, 0, 1.0m, 1.0m);
        score.Should().Be(100);
    }

    [Fact]
    public void ComputeCompositeScore_AllNegativeAndLongCommute_ShouldReturnZero()
    {
        var score = _engine.ComputeCompositeScore(-0.5m, 3600, 3600, -0.3m, -0.2m);
        score.Should().Be(0);
    }
}