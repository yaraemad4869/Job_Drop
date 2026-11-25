using Xunit;
using FluentAssertions;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Services;

namespace JobDrop.Domain.UnitTests.Services;

public class IntervalExperienceCalculatorTests
{
    private readonly IntervalExperienceCalculator _calculator = new();

    [Fact]
    public void ComputeNonOverlappingMonths_NoExperiences_ShouldReturnZero()
    {
        var months = _calculator.ComputeNonOverlappingMonths(Enumerable.Empty<WorkExperience>());
        months.Should().Be(0);
    }

    [Fact]
    public void ComputeNonOverlappingMonths_SingleExperience_ShouldCalculateCorrectly()
    {
        var experiences = new[]
        {
            new WorkExperience
            {
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 6, 30)
            }
        };
        _calculator.ComputeNonOverlappingMonths(experiences).Should().Be(6);
    }

    [Fact]
    public void ComputeNonOverlappingMonths_OverlappingExperiences_ShouldNotDoubleCount()
    {
        var experiences = new[]
        {
            new WorkExperience { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 6, 30) },
            new WorkExperience { StartDate = new DateOnly(2024, 4, 1), EndDate = new DateOnly(2024, 9, 30) }
        };
        _calculator.ComputeNonOverlappingMonths(experiences).Should().Be(9);
    }

    [Fact]
    public void ComputeNonOverlappingMonths_CurrentJob_ShouldIncludeUpToToday()
    {
        var experiences = new[]
        {
            new WorkExperience { StartDate = new DateOnly(2024, 1, 1), EndDate = null }
        };
        _calculator.ComputeNonOverlappingMonths(experiences).Should().BeGreaterThan(0);
    }

    [Fact]
    public void ComputeNonOverlappingMonths_AdjacentExperiences_ShouldMerge()
    {
        var experiences = new[]
        {
            new WorkExperience { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 3, 31) },
            new WorkExperience { StartDate = new DateOnly(2024, 4, 1), EndDate = new DateOnly(2024, 6, 30) }
        };
        _calculator.ComputeNonOverlappingMonths(experiences).Should().Be(6);
    }

    [Fact]
    public void ComputeNonOverlappingMonths_ParallelJobs_ShouldCountOnce()
    {
        var experiences = new[]
        {
            new WorkExperience { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31) },
            new WorkExperience { StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31) }
        };
        _calculator.ComputeNonOverlappingMonths(experiences).Should().Be(12);
    }
}