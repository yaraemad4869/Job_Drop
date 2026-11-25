using Xunit;
using FluentAssertions;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Companies.Commands;
using JobDrop.Domain.Exceptions;
using NSubstitute;

namespace JobDrop.Application.UnitTests.Features.Companies.Commands;

// NOTE: Membership/authorization checks hit the database and are covered by Integration Tests.
public class CreateCompanyOfficeHandlerTests
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly CreateCompanyOfficeHandler _handler;

    public CreateCompanyOfficeHandlerTests()
    {
        _db = Substitute.For<IApplicationDbContext>();
        _currentUser = Substitute.For<ICurrentUser>();
        _handler = new CreateCompanyOfficeHandler(_db, _currentUser);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowForbiddenAccessException()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new CreateCompanyOfficeCommand(Guid.NewGuid(), "HQ", 0, 30.0444, 31.2357, 500, null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldNotPersist()
    {
        _currentUser.UserId.Returns((Guid?)null);
        var command = new CreateCompanyOfficeCommand(Guid.NewGuid(), "HQ", 0, 30.0444, 31.2357, 500, null);

        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        await _db.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}