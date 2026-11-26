namespace JobDrop.Domain.Entities.Identity;

/// <summary>
/// Refresh token with one-time rotation and family-based reuse detection.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public Guid FamilyId { get; private set; } = Guid.NewGuid();
    public string? DeviceInfo { get; private set; }
    public string? IpAddress { get; private set; }
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;
    public bool IsRevoked => RevokedAtUtc.HasValue;

    public static RefreshToken Create(Guid userId, string tokenHash, string? deviceInfo, string? ipAddress, TimeSpan lifetime)
    {
        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            ExpiresAtUtc = DateTimeOffset.UtcNow.Add(lifetime)
        };
    }

    public void Revoke()
    {
        RevokedAtUtc = DateTimeOffset.UtcNow;
    }

    public bool IsExpired() => DateTimeOffset.UtcNow >= ExpiresAtUtc;
}