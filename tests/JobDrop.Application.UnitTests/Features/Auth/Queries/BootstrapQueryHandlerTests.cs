using Xunit;
using FluentAssertions;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Auth.Queries;
using JobDrop.Domain.Exceptions;
using NSubstitute;

namespace JobDrop.Application.UnitTests.Features.Auth.Queries;

public class BootstrapQueryHandlerTests
{
    private readonly ICurrentUser _currentUser;
    private readonly IApplicationDbContext _db;
    private readonly IDateTimeProvider _time;
    private readonly BootstrapQueryHandler _handler;

    public BootstrapQueryHandlerTests()
    {
        _currentUser = Substitute.For<ICurrentUser>();
        _db = Substitute.For<IApplicationDbContext>();
        _time = Substitute.For<IDateTimeProvider>();
        _handler = new BootstrapQueryHandler(_currentUser, _db, _time);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowForbiddenAccessException()
    {
        _currentUser.UserId.Returns((Guid?)null);

        var act = async () => await _handler.Handle(new BootstrapQuery(), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}