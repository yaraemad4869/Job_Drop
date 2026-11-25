using FluentAssertions;
using JobDrop.Application.Features.Companies.Commands;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Companies.Commands;

public class CreateCompanyOfficeCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new CreateCompanyOfficeCommandValidator();
        var command = new CreateCompanyOfficeCommand(
            CompanyId: Guid.NewGuid(),
            Nickname: "Main Office",
            OfficeType: "HQ",
            Latitude: 30.0444,
            Longitude: 31.2357,
            GeofenceRadiusMeters: 500,
            TransitAnchorText: "Near Metro");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidOfficeType_ShouldFail()
    {
        // Arrange
        var validator = new CreateCompanyOfficeCommandValidator();
        var command = new CreateCompanyOfficeCommand(
            CompanyId: Guid.NewGuid(),
            Nickname: "Main Office",
            OfficeType: "Invalid",
            Latitude: 30.0444,
            Longitude: 31.2357,
            GeofenceRadiusMeters: 500,
            TransitAnchorText: null);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithInvalidLatitude_ShouldFail()
    {
        // Arrange
        var validator = new CreateCompanyOfficeCommandValidator();
        var command = new CreateCompanyOfficeCommand(
            CompanyId: Guid.NewGuid(),
            Nickname: "Main Office",
            OfficeType: "HQ",
            Latitude: 95.0,
            Longitude: 31.2357,
            GeofenceRadiusMeters: 500,
            TransitAnchorText: null);

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
