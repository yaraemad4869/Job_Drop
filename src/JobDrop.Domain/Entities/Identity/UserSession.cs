using System.ComponentModel.DataAnnotations.Schema;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Identity;

public sealed class UserSession : BaseEntity
{
    public Guid UserId { get; set; }
    public string RefreshTokenHash { get; set; } = string.Empty;
    public Guid FamilyId { get; set; } = Guid.CreateVersion7();
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    [NotMapped]
    public bool IsRevoked => RevokedAtUtc.HasValue;

    [NotMapped]
    public bool IsExpired => ExpiresAtUtc <= DateTimeOffset.UtcNow;

    [NotMapped]
    public bool IsActive => !IsRevoked && !IsExpired;
}