using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokens;

    public LoginCommandHandler(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ITokenService tokens)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokens = tokens;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim().ToLowerInvariant())
            ?? throw new DomainRuleException("Invalid credentials.", "INVALID_CREDENTIALS");

        if (!user.IsActive)
            throw new DomainRuleException("Account is deactivated.", "ACCOUNT_DEACTIVATED");

        if (!user.EmailConfirmed)
            throw new DomainRuleException("Email is not confirmed.", "EMAIL_NOT_CONFIRMED");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            throw new DomainRuleException("Account is locked. Try again later.", "ACCOUNT_LOCKED");

        if (!result.Succeeded)
            throw new DomainRuleException("Invalid credentials.", "INVALID_CREDENTIALS");

        user.LastLoginAtUtc = DateTimeOffset.UtcNow;
        await _userManager.UpdateAsync(user);

        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        var tokens = await _tokens.GenerateTokensAsync(user, roles, request.DeviceInfo, cancellationToken);

        return new AuthResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAtUtc,
            user.Handle,
            user.Email ?? string.Empty,
            roles);
    }
}