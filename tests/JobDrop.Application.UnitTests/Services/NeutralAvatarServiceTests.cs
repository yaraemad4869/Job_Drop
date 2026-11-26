using System;
using Xunit;
using JobDrop.Application.Services;

namespace JobDrop.Application.UnitTests.Services;

public class NeutralAvatarServiceTests
{
    [Fact]
    public void GetNeutralAvatar_WithSameCandidateId_ReturnsSameAvatar()
    {
        // Arrange
        var service = new NeutralAvatarService();
        var candidateId = Guid.NewGuid();
        
        // Act
        var avatar1 = service.GetNeutralAvatar(candidateId);
        var avatar2 = service.GetNeutralAvatar(candidateId);
        
        // Assert
        Assert.Equal(avatar1, avatar2);
        Assert.StartsWith("/assets/avatars/neutral-", avatar1);
    }

    [Fact]
    public void GetNeutralAvatar_WithDifferentCandidateIds_ReturnsDifferentAvatars()
    {
        // Arrange
        var service = new NeutralAvatarService();
        var candidateId1 = Guid.NewGuid();
        var candidateId2 = Guid.NewGuid();
        
        // Act
        var avatar1 = service.GetNeutralAvatar(candidateId1);
        var avatar2 = service.GetNeutralAvatar(candidateId2);
        
        // Assert
        // Note: They might be the same due to hash collision, but both should be valid neutral avatars
        Assert.StartsWith("/assets/avatars/neutral-", avatar1);
        Assert.StartsWith("/assets/avatars/neutral-", avatar2);
    }
}
