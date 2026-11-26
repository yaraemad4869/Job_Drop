using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Services;

namespace JobDrop.Application.UnitTests.Services;

public class CompanyRankingServiceTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly CompanyRankingService _service;

    public CompanyRankingServiceTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _service = new CompanyRankingService(_mockContext.Object);
    }

    [Fact]
    public void CalculateOrdinalRank_WithValidData_ReturnsCorrectRank()
    {
        // Arrange
        var followUpRate = 0.85;
        var allRates = new List<double> { 0.95, 0.90, 0.85, 0.80, 0.75 };
        
        // Act
        var rank = _service.CalculateOrdinalRank(followUpRate, allRates);
        
        // Assert
        Assert.Equal(3, rank);
    }

    [Fact]
    public void IsEligibleForDisplay_WithSufficientSample_ReturnsTrue()
    {
        // Arrange & Act
        var result = _service.IsEligibleForDisplay(1, 50, 10);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEligibleForDisplay_WithInsufficientSample_ReturnsFalse()
    {
        // Arrange & Act
        var result = _service.IsEligibleForDisplay(1, 5, 10);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CalculatePercentile_WithValidData_ReturnsCorrectPercentile()
    {
        // Arrange & Act
        var percentile = _service.CalculatePercentile(1, 100);
        
        // Assert
        Assert.Equal(100.0, percentile);
    }
}
