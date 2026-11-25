using Xunit;
using FluentAssertions;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.UnitTests.ValueObjects;

public class TimeWindowTests
{
    [Fact]
    public void Constructor_ValidWindow_ShouldCreate()
    {
        var start = new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero);
        var end = new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero);
        var window = new TimeWindow(start, end);
        window.StartUtc.Should().Be(start);
        window.EndUtc.Should().Be(end);
        window.Duration.Should().Be(TimeSpan.FromHours(8));
    }

    [Fact]
    public void Constructor_EndBeforeStart_ShouldThrow()
    {
        var start = new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero);
        var end = new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero);
        var act = () => new TimeWindow(start, end);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_EqualStartEnd_ShouldThrow()
    {
        var time = new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero);
        var act = () => new TimeWindow(time, time);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Contains_InstantInsideWindow_ShouldReturnTrue()
    {
        var window = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero));
        var instant = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
        window.Contains(instant).Should().BeTrue();
    }

    [Fact]
    public void Contains_InstantOutsideWindow_ShouldReturnFalse()
    {
        var window = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero));
        var instant = new DateTimeOffset(2024, 1, 1, 18, 0, 0, TimeSpan.Zero);
        window.Contains(instant).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_OverlappingWindows_ShouldReturnTrue()
    {
        var window1 = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero));
        var window2 = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 20, 0, 0, TimeSpan.Zero));
        window1.Overlaps(window2).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_NonOverlappingWindows_ShouldReturnFalse()
    {
        var window1 = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 9, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero));
        var window2 = new TimeWindow(
            new DateTimeOffset(2024, 1, 1, 13, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 17, 0, 0, TimeSpan.Zero));
        window1.Overlaps(window2).Should().BeFalse();
    }
}