using JobDrop.Domain.Entities.Identity;

namespace JobDrop.Application.Contracts.Identity;

public sealed record AuthTokens(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiresAtUtc);

public interface ITokenService
{
    Task<AuthTokens> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, string? deviceInfo, CancellationToken cancellationToken = default);
    Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
}