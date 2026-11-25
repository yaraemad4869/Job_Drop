using Xunit;
using FluentAssertions;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.Commands;
using JobDrop.Domain.Exceptions;
using NSubstitute;

namespace JobDrop.Application.UnitTests.Features.Candidates.Commands;

// NOTE: Database-dependent paths (ProfileNotFound) are covered by Integration Tests.
// NSubstitute cannot mock EF Core async queries (IAsyncQueryProvider).
public class UpdateCandidateProfileHandlerTests
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly UpdateCandidateProfileHandler _handler;

    public UpdateCandidateProfileHandlerTests()
    {
        _db = Substitute.For<IApplicationDbContext>();
        _currentUser = Substitute.For<ICurrentUser>();
        _handler = new UpdateCandidateProfileHandler(_db, _currentUser);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowForbiddenAccessException()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data("New Headline", null, null, null));

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldNotTouchDatabase()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new UpdateCandidateProfileCommand(
            new UpdateCandidateProfileCommand.Data("X", null, null, null));

        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        await _db.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}