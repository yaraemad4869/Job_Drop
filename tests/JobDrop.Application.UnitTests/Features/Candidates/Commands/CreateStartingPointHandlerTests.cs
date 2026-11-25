using FluentAssertions;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.Commands;
using NSubstitute;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

public class CreateStartingPointHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateStartingPoint()
    {
        // Arrange
        var context = Substitute.For<IApplicationDbContext>();
        var handler = new CreateStartingPointHandler(context);

        var command = new CreateStartingPointCommand(
            CandidateProfileId: Guid.NewGuid(),
            Label: "Home",
            AnchorType: "Primary",
            Latitude: 30.0444,
            Longitude: 31.2357,
            IsActiveOrigin: true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        await context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
