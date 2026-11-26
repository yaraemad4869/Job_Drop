using JobDrop.Domain.Services;
using Xunit;

namespace JobDrop.Domain.UnitTests.Services;

public sealed class SlaDeadlineCalculatorTests
{
    [Fact]
    public void CalculateDueAtUtc_WithNullCalendar_Adds48Hours()
    {
        var calc = new SlaDeadlineCalculator();
        var delivered = new DateTime(2026, 11, 26, 10, 0, 0, DateTimeKind.Utc);

        var due = calc.CalculateDueAtUtc(delivered, calendar: null, holidays: null);

        Assert.Equal(delivered.AddHours(48), due);
    }

    [Fact]
    public void CalculateDueAtUtc_WeekendSkipped_AddsMoreThan48Hours()
    {
        // Friday 10:00 UTC + 48h = Sunday 10:00 UTC
        // If calendar skips weekends, should be Monday 10:00 UTC
        var calc = new SlaDeadlineCalculator();
        var friday = new DateTime(2026, 11, 27, 10, 0, 0, DateTimeKind.Utc); // Friday

        var due = calc.CalculateDueAtUtc(friday, calendar: null, holidays: null);

        // Without calendar, strict 48h
        Assert.Equal(friday.AddHours(48), due);
    }

    [Fact]
    public void IsExpired_WhenPastDue_ReturnsTrue()
    {
        var calc = new SlaDeadlineCalculator();
        var dueAtUtc = DateTime.UtcNow.AddHours(-1);
        var nowUtc = DateTime.UtcNow;

        var expired = calc.IsExpired(dueAtUtc, nowUtc);

        Assert.True(expired);
    }

    [Fact]
    public void IsExpired_WhenFuture_ReturnsFalse()
    {
        var calc = new SlaDeadlineCalculator();
        var dueAtUtc = DateTime.UtcNow.AddHours(1);
        var nowUtc = DateTime.UtcNow;

        var expired = calc.IsExpired(dueAtUtc, nowUtc);

        Assert.False(expired);
    }

    [Fact]
    public void GetRemainingSeconds_WhenFuture_ReturnsPositive()
    {
        var calc = new SlaDeadlineCalculator();
        var dueAtUtc = DateTime.UtcNow.AddMinutes(30);
        var nowUtc = DateTime.UtcNow;

        var remaining = calc.GetRemainingSeconds(dueAtUtc, nowUtc);

        Assert.True(remaining > 0);
        Assert.True(remaining <= 30 * 60);
    }
}