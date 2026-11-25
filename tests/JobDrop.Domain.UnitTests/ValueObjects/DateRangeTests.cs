using Xunit;
using FluentAssertions;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.UnitTests.ValueObjects;

public class DateRangeTests
{
    [Fact]
    public void Constructor_ValidRange_ShouldCreate()
    {
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 12, 31);
        var range = new DateRange(start, end);
        range.StartDate.Should().Be(start);
        range.EndDate.Should().Be(end);
        range.IsCurrent.Should().BeFalse();
    }

    [Fact]
    public void Constructor_NullEndDate_ShouldBeCurrent()
    {
        var start = new DateOnly(2024, 1, 1);
        var range = new DateRange(start, null);
        range.IsCurrent.Should().BeTrue();
    }

    [Fact]
    public void Constructor_EndBeforeStart_ShouldThrow()
    {
        var start = new DateOnly(2024, 12, 31);
        var end = new DateOnly(2024, 1, 1);
        var act = () => new DateRange(start, end);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ComputeOverlappingMonths_OverlappingRanges_ShouldCalculateCorrectly()
    {
        var range1 = new DateRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 6, 30));
        var range2 = new DateRange(new DateOnly(2024, 4, 1), new DateOnly(2024, 9, 30));
        range1.ComputeOverlappingMonths(range2).Should().Be(3);
    }

    [Fact]
    public void ComputeOverlappingMonths_NoOverlap_ShouldReturnZero()
    {
        var range1 = new DateRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 3, 31));
        var range2 = new DateRange(new DateOnly(2024, 5, 1), new DateOnly(2024, 7, 31));
        range1.ComputeOverlappingMonths(range2).Should().Be(0);
    }

    [Fact]
    public void ComputeOverlappingMonths_SameRange_ShouldReturnFullMonths()
    {
        var range1 = new DateRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 6, 30));
        var range2 = new DateRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 6, 30));
        range1.ComputeOverlappingMonths(range2).Should().Be(6);
    }
}