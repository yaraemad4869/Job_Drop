using System;
using System.Reflection;
using Xunit;
using JobDrop.Application.Features.Candidates.Services;
using JobDrop.Domain.Entities.Candidates;

namespace JobDrop.Application.UnitTests.Services;

public class BlindModeServiceTests
{
    [Fact]
    public void GenerateCandidateCode_WithSameIds_ReturnsSameCode()
    {
        // Arrange
        var service = new BlindModeService();
        var candidateId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        
        // Act
        var code1 = service.GenerateCandidateCode(candidateId, companyId);
        var code2 = service.GenerateCandidateCode(candidateId, companyId);
        
        // Assert
        Assert.Equal(code1, code2);
        Assert.StartsWith("C-", code1);
    }

    [Fact]
    public void GenerateCandidateCode_WithDifferentIds_ReturnsDifferentCodes()
    {
        // Arrange
        var service = new BlindModeService();
        var candidateId1 = Guid.NewGuid();
        var candidateId2 = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        
        // Act
        var code1 = service.GenerateCandidateCode(candidateId1, companyId);
        var code2 = service.GenerateCandidateCode(candidateId2, companyId);
        
        // Assert
        Assert.NotEqual(code1, code2);
    }

    [Fact]
    public void ToBlindDto_WithValidProfile_ReturnsBlindDto()
    {
        // Arrange
        var service = new BlindModeService();
        var userId = Guid.NewGuid();
        
        // Use the correct constructor with required parameters
        var profile = new CandidateProfile(userId, "test@example.com", null);
        
        // Use reflection to set the Id if needed
        var idProperty = typeof(CandidateProfile).BaseType?.GetProperty("Id");
        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(profile, userId);
        }
        
        var companyId = Guid.NewGuid();
        
        // Act
        var dto = service.ToBlindDto(profile, companyId, 85.0, "High", new[] { "C#", ".NET" }, 5);
        
        // Assert
        Assert.NotNull(dto);
        Assert.StartsWith("C-", dto.CandidateCode);
        Assert.Equal(85.0, dto.MatchScore);
        Assert.Equal("High", dto.MatchBand);
        Assert.Equal(2, dto.Skills.Count);
        Assert.Equal(5, dto.ExperienceYears);
        Assert.Equal("/assets/avatars/neutral.svg", dto.AvatarUrl);
    }
}
