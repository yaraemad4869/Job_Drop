using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Companies.Commands;

namespace JobDrop.Application.UnitTests.Features.Companies.Commands;

public class CreateCompanyOfficeCommandValidatorTests
{
    private readonly CreateCompanyOfficeCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new CreateCompanyOfficeCommand(
            Guid.NewGuid(), "HQ Cairo", 0, 30.0444, 31.2357, 500, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void EmptyCompanyId_ShouldFail()
    {
        var command = new CreateCompanyOfficeCommand(
            Guid.Empty, "HQ", 0, 30.0444, 31.2357, 500, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void EmptyNickname_ShouldFail()
    {
        var command = new CreateCompanyOfficeCommand(
            Guid.NewGuid(), "", 0, 30.0444, 31.2357, 500, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(49)]
    [InlineData(50001)]
    public void InvalidGeofenceRadius_ShouldFail(int radius)
    {
        var command = new CreateCompanyOfficeCommand(
            Guid.NewGuid(), "HQ", 0, 30.0444, 31.2357, radius, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}