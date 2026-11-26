using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobDrop.Infrastructure.Identity;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<AuthTokens> GenerateTokensAsync(
        AppUser user,
        IEnumerable<string> roles,
        string? deviceId,
        CancellationToken cancellationToken)
    {
        var secretKey = _configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey is not configured");

        if (secretKey.Length < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey must be at least 32 characters");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (!string.IsNullOrEmpty(user.Handle))
        {
            claims.Add(new Claim("handle", user.Handle));
        }

        var accessTokenExpiresAtUtc = DateTimeOffset.UtcNow.AddHours(1);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "JobDrop",
            audience: _configuration["Jwt:Audience"] ?? "JobDrop",
            claims: claims,
            expires: accessTokenExpiresAtUtc.UtcDateTime,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return Task.FromResult(new AuthTokens(accessToken, refreshToken, accessTokenExpiresAtUtc));
    }

    public Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        // TODO: Implement refresh token validation against database
        // For now, return null to indicate invalid
        return Task.FromResult<Guid?>(null);
    }

    public Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        // TODO: Implement refresh token revocation
        return Task.CompletedTask;
    }

    public Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Implement session revocation
        return Task.CompletedTask;
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
