using FluentAssertions;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Companies.Commands;
using NSubstitute;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Companies.Commands;

public class CreateCompanyOfficeHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateOffice()
    {
        // Arrange
        var context = Substitute.For<IApplicationDbContext>();
        var handler = new CreateCompanyOfficeHandler(context);

        var command = new CreateCompanyOfficeCommand(
            CompanyId: Guid.NewGuid(),
            Nickname: "Main Office",
            OfficeType: "HQ",
            Latitude: 30.0444,
            Longitude: 31.2357,
            GeofenceRadiusMeters: 500,
            TransitAnchorText: "Near Metro");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
