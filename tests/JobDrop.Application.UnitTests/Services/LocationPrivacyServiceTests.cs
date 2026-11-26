using System;
using Xunit;
using JobDrop.Application.Services;

namespace JobDrop.Application.UnitTests.Services;

public class LocationPrivacyServiceTests
{
    [Fact]
    public void RoundForSharing_WithValidCoordinates_ReturnsRoundedCoordinates()
    {
        // Arrange
        var service = new LocationPrivacyService();
        var lat = 30.0444;
        var lon = 31.2357;
        
        // Act
        var (roundedLat, roundedLon) = service.RoundForSharing(lat, lon, 500);
        
        // Assert
        Assert.NotEqual(lat, roundedLat);
        Assert.NotEqual(lon, roundedLon);
        Assert.True(Math.Abs(roundedLat - lat) < 0.01);
        Assert.True(Math.Abs(roundedLon - lon) < 0.01);
    }

    [Fact]
    public void IsDifferentialPrivacyClaimValid_ReturnsFalse()
    {
        // Arrange
        var service = new LocationPrivacyService();
        
        // Act
        var result = service.IsDifferentialPrivacyClaimValid();
        
        // Assert
        Assert.False(result);
    }
}
