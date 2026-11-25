using Xunit;
using FluentAssertions;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Features.Auth.Commands;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace JobDrop.Application.UnitTests.Features.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        var userStore = Substitute.For<IUserStore<AppUser>>();
        _userManager = Substitute.For<UserManager<AppUser>>(
            userStore, null!, null!, null!, null!, null!, null!, null!, null!);
        var contextAccessor = Substitute.For<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<AppUser>>();
        _signInManager = Substitute.For<SignInManager<AppUser>>(
            _userManager, contextAccessor, userPrincipalFactory, null!, null!, null!, null!);
        _tokenService = Substitute.For<ITokenService>();
        _handler = new LoginCommandHandler(_signInManager, _userManager, _tokenService);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowDomainRuleException()
    {
        var command = new LoginCommand("notfound@example.com", "password", null);
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns((AppUser?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainRuleException>()
            .WithMessage("*Invalid credentials*");
    }

    [Fact]
    public async Task Handle_InactiveUser_ShouldThrowDomainRuleException()
    {
        var user = new AppUser { Email = "test@example.com", IsActive = false, EmailConfirmed = true };
        var command = new LoginCommand("test@example.com", "password", null);
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(user);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainRuleException>()
            .WithMessage("*deactivated*");
    }

    [Fact]
    public async Task Handle_UnconfirmedEmail_ShouldThrowDomainRuleException()
    {
        var user = new AppUser { Email = "test@example.com", IsActive = true, EmailConfirmed = false };
        var command = new LoginCommand("test@example.com", "password", null);
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(user);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainRuleException>()
            .WithMessage("*not confirmed*");
    }
}