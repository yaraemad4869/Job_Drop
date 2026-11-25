using FluentAssertions;
using JobDrop.Application.Features.Candidates.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class CreateStartingPointCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new CreateStartingPointCommandValidator();
        var command = new CreateStartingPointCommand(
            CandidateProfileId: Guid.NewGuid(),
            Label: "Home",
            AnchorType: "Primary",
            Latitude: 30.0444,
            Longitude: 31.2357,
            IsActiveOrigin: true);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidAnchorType_ShouldFail()
    {
        // Arrange
        var validator = new CreateStartingPointCommandValidator();
        var command = new CreateStartingPointCommand(
            CandidateProfileId: Guid.NewGuid(),
            Label: "Home",
            AnchorType: "Invalid",
            Latitude: 30.0444,
            Longitude: 31.2357,
            IsActiveOrigin: true);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidLatitude_ShouldFail()
    {
        // Arrange
        var validator = new CreateStartingPointCommandValidator();
        var command = new CreateStartingPointCommand(
            CandidateProfileId: Guid.NewGuid(),
            Label: "Home",
            AnchorType: "Primary",
            Latitude: 95.0,
            Longitude: 31.2357,
            IsActiveOrigin: true);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithEmptyLabel_ShouldFail()
    {
        // Arrange
        var validator = new CreateStartingPointCommandValidator();
        var command = new CreateStartingPointCommand(
            CandidateProfileId: Guid.NewGuid(),
            Label: "",
            AnchorType: "Primary",
            Latitude: 30.0444,
            Longitude: 31.2357,
            IsActiveOrigin: true);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
