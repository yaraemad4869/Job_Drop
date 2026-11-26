using JobDrop.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace JobDrop.Domain.Entities.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    // JobDrop-specific
    public string Handle { get; set; } = string.Empty;
    public AvatarCode AvatarCode { get; set; } = AvatarCode.Neutral;
    public string? GoogleSubjectId { get; set; }
    public int TokenVersion { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public Language PreferredLanguage { get; set; } = Language.English;
    public Theme PreferredTheme { get; set; } = Theme.DesertCream;
    public string Currency { get; set; } = "USD";
    public string TimeZoneId { get; set; } = "UTC";
    public CalendarType CalendarType { get; set; } = CalendarType.Gregorian;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastLoginAtUtc { get; set; }
}
