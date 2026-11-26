using System;
using System.Collections.Generic;
using Xunit;
using JobDrop.Application.Services;
using JobDrop.Domain.Enums;

namespace JobDrop.Application.UnitTests.Services;

public class MultiModeCommuteServiceTests
{
    [Fact]
    public void JobQualifies_WhenAnyModeSatisfiesThreshold_ReturnsTrue()
    {
        // Arrange
        var service = new MultiModeCommuteService();
        var modeResults = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 30 },
            { CommuteMode.Transit, 60 }
        };
        var thresholds = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 45 },
            { CommuteMode.Transit, 45 }
        };
        
        // Act
        var result = service.JobQualifies(modeResults, thresholds);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void JobQualifies_WhenNoModeSatisfiesThreshold_ReturnsFalse()
    {
        // Arrange
        var service = new MultiModeCommuteService();
        var modeResults = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 60 },
            { CommuteMode.Transit, 90 }
        };
        var thresholds = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 45 },
            { CommuteMode.Transit, 45 }
        };
        
        // Act
        var result = service.JobQualifies(modeResults, thresholds);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetPrimaryModeForSorting_WithMultipleModes_ReturnsShortestTime()
    {
        // Arrange
        var service = new MultiModeCommuteService();
        var modeResults = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 30 },
            { CommuteMode.Transit, 45 }
        };
        var thresholds = new Dictionary<CommuteMode, int>
        {
            { CommuteMode.Driving, 45 },
            { CommuteMode.Transit, 60 }
        };
        
        // Act
        var result = service.GetPrimaryModeForSorting(modeResults, thresholds);
        
        // Assert
        Assert.Equal(CommuteMode.Driving, result);
    }
}
