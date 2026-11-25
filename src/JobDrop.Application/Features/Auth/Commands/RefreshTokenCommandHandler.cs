using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly ITokenService _tokens;
    private readonly UserManager<AppUser> _userManager;

    public RefreshTokenCommandHandler(ITokenService tokens, UserManager<AppUser> userManager)
    {
        _tokens = tokens;
        _userManager = userManager;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = await _tokens.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (!userId.HasValue)
            throw new DomainRuleException("Invalid or expired refresh token.", "INVALID_REFRESH_TOKEN");

        var user = await _userManager.FindByIdAsync(userId.Value.ToString())
            ?? throw new NotFoundException(nameof(AppUser), userId.Value);

        if (!user.IsActive)
            throw new DomainRuleException("Account is deactivated.", "ACCOUNT_DEACTIVATED");

        await _tokens.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);

        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        var newTokens = await _tokens.GenerateTokensAsync(user, roles, request.DeviceInfo, cancellationToken);

        return new AuthResponse(
            newTokens.AccessToken,
            newTokens.RefreshToken,
            newTokens.AccessTokenExpiresAtUtc,
            user.Handle,
            user.Email ?? string.Empty,
            roles);
    }
}