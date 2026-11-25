using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JobDrop.Infrastructure.Identity;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "JobDrop";
    public string Audience { get; set; } = "JobDrop";
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenLifetimeMinutes { get; set; } = 15;
    public int RefreshTokenLifetimeDays { get; set; } = 30;
}

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly ApplicationDbContext _db;

    public JwtTokenService(IOptions<JwtOptions> options, ApplicationDbContext db)
    {
        _options = options.Value;
        _db = db;
    }

    public async Task<AuthTokens> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, string? deviceInfo, CancellationToken cancellationToken = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new("handle", user.Handle),
            new("tv", user.TokenVersion.ToString())
        };
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshTokenBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(refreshTokenBytes);
        var refreshTokenHash = HashRefreshToken(refreshToken);

        var session = new UserSession
        {
            UserId = user.Id,
            RefreshTokenHash = refreshTokenHash,
            DeviceInfo = deviceInfo,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenLifetimeDays),
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _db.UserSessions.Add(session);
        await _db.SaveChangesAsync(cancellationToken);

        return new AuthTokens(accessToken, refreshToken, new DateTimeOffset(expires, TimeSpan.Zero));
    }

    public async Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var hash = HashRefreshToken(refreshToken);
        var session = await _db.UserSessions
            .Where(s => s.RefreshTokenHash == hash && s.RevokedAtUtc == null && s.ExpiresAtUtc > DateTimeOffset.UtcNow)
            .Select(s => new { s.UserId, s.Id })
            .FirstOrDefaultAsync(cancellationToken);

        return session?.UserId;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var hash = HashRefreshToken(refreshToken);
        var session = await _db.UserSessions
            .Where(s => s.RefreshTokenHash == hash && s.RevokedAtUtc == null)
            .FirstOrDefaultAsync(cancellationToken);

        if (session is not null)
        {
            session.RevokedAtUtc = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var sessions = await _db.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var s in sessions) s.RevokedAtUtc = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static string HashRefreshToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}