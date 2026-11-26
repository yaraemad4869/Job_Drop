using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using JobDrop.Application.Features.Drops.Validators;
using JobDrop.Domain.Enums;

namespace JobDrop.Application.UnitTests.Services;

public class SLADeadlineCalculatorTests
{
    [Fact]
    public void CalculateDueAtUtc_WithDeliveredAt_ReturnsCorrectDeadline()
    {
        // Arrange
        var deliveredAt = new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero);
        
        // Act
        var dueAt = SLADeadlineCalculator.CalculateDueAtUtc(deliveredAt, "v1");
        
        // Assert
        Assert.NotNull(dueAt);
        Assert.Equal(deliveredAt.AddHours(48), dueAt);
    }

    [Fact]
    public void CalculateDueAtUtc_WithNull_ReturnsNull()
    {
        // Act
        var dueAt = SLADeadlineCalculator.CalculateDueAtUtc(null, "v1");
        
        // Assert
        Assert.Null(dueAt);
    }

    [Theory]
    [InlineData(95.0, MatchBand.High)]
    [InlineData(90.0, MatchBand.High)]
    [InlineData(89.9, MatchBand.Medium)]
    [InlineData(85.0, MatchBand.Medium)]
    [InlineData(80.0, MatchBand.Medium)]
    [InlineData(79.9, MatchBand.Low)]
    [InlineData(50.0, MatchBand.Low)]
    public void ClassifyMatchScore_WithVariousScores_ReturnsCorrectBand(double score, MatchBand expected)
    {
        // Act
        var result = SLADeadlineCalculator.ClassifyMatchScore(score);
        
        // Assert
        Assert.Equal(expected, result);
    }
}
