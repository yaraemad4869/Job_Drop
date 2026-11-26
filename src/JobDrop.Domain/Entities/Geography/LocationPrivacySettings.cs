namespace JobDrop.Domain.Entities.Geography;

/// <summary>
/// BE-17: Location precision and privacy settings.
/// Separates internal storage precision from shared precision.
/// </summary>
public class LocationPrivacySettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public int InternalPrecisionMeters { get; set; } = 1; // Exact for internal use
    public int SharedPrecisionMeters { get; set; } = 500; // Rounded for recruiters
    public bool AllowResidentialPoint { get; set; } = false;
    public bool AllowExactLocation { get; set; } = false;
    public string RetentionPurpose { get; set; } = "Matching";
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
