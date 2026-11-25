using Xunit;
using FluentAssertions;
using JobDrop.Application.Features.Discovery.DTOs;
using JobDrop.Application.Features.Discovery.Queries;

namespace JobDrop.Application.UnitTests.Features.Discovery.Queries;

public class SpatialSearchHandlerTests
{
    private readonly SpatialSearchHandler _handler = new();

    [Fact]
    public async Task Handle_ShouldReturnMockData()
    {
        var command = new SpatialSearchCommand(
            TargetRoles: null, SkillIds: null, WorkModes: null,
            OriginType: null, OriginId: null,
            CustomLatitude: null, CustomLongitude: null,
            CommuteMode: null, MaxDurationMinutes: null, RadiusKm: null,
            SalaryMin: null, SalaryMax: null,
            VerifiedCompaniesOnly: false, ShowBeyondCommuteLimit: false);
        var query = new SpatialSearchQuery(command);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.SearchContextId.Should().NotBeNullOrEmpty();
        result.Jobs.Should().NotBeNull();
        result.Jobs.Count.Should().Be(2);
    }
}