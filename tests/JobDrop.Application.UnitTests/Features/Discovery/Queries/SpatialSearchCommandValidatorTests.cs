using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Discovery.DTOs;
using JobDrop.Application.Features.Discovery.Queries;

namespace JobDrop.Application.UnitTests.Features.Discovery.Queries;

public class SpatialSearchCommandValidatorTests
{
    private readonly SpatialSearchCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_ShouldPass()
    {
        var command = new SpatialSearchCommand(
            TargetRoles: new List<string> { "Developer" },
            SkillIds: null,
            WorkModes: null,
            OriginType: "StartingPoint",
            OriginId: Guid.NewGuid(),
            CustomLatitude: null,
            CustomLongitude: null,
            CommuteMode: 0,
            MaxDurationMinutes: 60,
            RadiusKm: 10,
            SalaryMin: 1000,
            SalaryMax: 2000,
            VerifiedCompaniesOnly: false,
            ShowBeyondCommuteLimit: false);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void TooManyTargetRoles_ShouldFail()
    {
        var command = new SpatialSearchCommand(
            TargetRoles: new List<string> { "R1", "R2", "R3", "R4", "R5", "R6" },
            SkillIds: null, WorkModes: null, OriginType: null, OriginId: null,
            CustomLatitude: null, CustomLongitude: null, CommuteMode: null,
            MaxDurationMinutes: null, RadiusKm: null, SalaryMin: null, SalaryMax: null,
            VerifiedCompaniesOnly: false, ShowBeyondCommuteLimit: false);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SalaryMaxLessThanMin_ShouldFail()
    {
        var command = new SpatialSearchCommand(
            TargetRoles: null, SkillIds: null, WorkModes: null, OriginType: null, OriginId: null,
            CustomLatitude: null, CustomLongitude: null, CommuteMode: null,
            MaxDurationMinutes: null, RadiusKm: null, SalaryMin: 2000, SalaryMax: 1000,
            VerifiedCompaniesOnly: false, ShowBeyondCommuteLimit: false);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}