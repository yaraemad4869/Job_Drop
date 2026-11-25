using Xunit;
using FluentAssertions;
using JobDrop.Domain.Services;

namespace JobDrop.Domain.UnitTests.Services;

public class SlaDeadlineCalculatorTests
{
    private readonly SlaDeadlineCalculator _calculator = new();

    [Fact]
    public void ComputeDeadline_StandardBusinessHours_ShouldCalculateCorrectly()
    {
        var startUtc = new DateTimeOffset(2024, 1, 15, 9, 0, 0, TimeSpan.Zero);
        var workingDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        var holidays = Array.Empty<DateOnly>();

        var deadline = _calculator.ComputeDeadline(startUtc, "UTC", workingDays, holidays, 540, 1020);

        deadline.Should().BeAfter(startUtc);
        var hoursDiff = (deadline - startUtc).TotalHours;
        hoursDiff.Should().BeApproximately(176, 2);
    }

    [Fact]
    public void ComputeDeadline_WithWeekend_ShouldSkipNonWorkingDays()
    {
        var startUtc = new DateTimeOffset(2024, 1, 19, 9, 0, 0, TimeSpan.Zero);
        var workingDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        var holidays = Array.Empty<DateOnly>();

        var deadline = _calculator.ComputeDeadline(startUtc, "UTC", workingDays, holidays, 540, 1020);

        deadline.DayOfWeek.Should().NotBe(DayOfWeek.Saturday);
        deadline.DayOfWeek.Should().NotBe(DayOfWeek.Sunday);
    }

    [Fact]
    public void ComputeDeadline_WithHoliday_ShouldSkipHoliday()
    {
        var startUtc = new DateTimeOffset(2024, 1, 15, 9, 0, 0, TimeSpan.Zero);
        var workingDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        var holidays = new[] { new DateOnly(2024, 1, 16) };

        var deadline = _calculator.ComputeDeadline(startUtc, "UTC", workingDays, holidays, 540, 1020);

        deadline.Should().BeAfter(startUtc.AddHours(48));
    }

    [Fact]
    public void ComputeDeadline_InvalidTimeZone_ShouldFallbackTo48Hours()
    {
        var startUtc = new DateTimeOffset(2024, 1, 15, 9, 0, 0, TimeSpan.Zero);
        var workingDays = new[] { DayOfWeek.Monday };
        var holidays = Array.Empty<DateOnly>();

        var deadline = _calculator.ComputeDeadline(startUtc, "Invalid/TimeZone", workingDays, holidays, 540, 1020);

        deadline.Should().Be(startUtc.AddHours(48));
    }

    [Fact]
    public void ComputeDeadline_StartAtEndOfDay_ShouldContinueNextDay()
    {
        var startUtc = new DateTimeOffset(2024, 1, 15, 17, 0, 0, TimeSpan.Zero);
        var workingDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        var holidays = Array.Empty<DateOnly>();

        var deadline = _calculator.ComputeDeadline(startUtc, "UTC", workingDays, holidays, 540, 1020);

        deadline.Should().BeAfter(startUtc);
    }
}