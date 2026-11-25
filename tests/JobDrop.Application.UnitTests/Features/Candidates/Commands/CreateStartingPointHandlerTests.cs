using Xunit;
using FluentAssertions;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.Commands;
using JobDrop.Domain.Exceptions;
using NSubstitute;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

// NOTE: Database-dependent paths (ProfileNotFound, capacity limit) are covered by Integration Tests.
public class CreateStartingPointHandlerTests
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly CreateStartingPointHandler _handler;

    public CreateStartingPointHandlerTests()
    {
        _db = Substitute.For<IApplicationDbContext>();
        _currentUser = Substitute.For<ICurrentUser>();
        _handler = new CreateStartingPointHandler(_db, _currentUser);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowForbiddenAccessException()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new CreateStartingPointCommand("Home", 0, 30.0444, 31.2357, true);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldNotPersist()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new CreateStartingPointCommand("Home", 0, 30.0444, 31.2357, true);

        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        await _db.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}