namespace JobDrop.Domain.Entities.Drops;

/// <summary>
/// BE-01: Tracks SLA policy version per application.
/// Policy changes must not silently change existing deadlines.
/// </summary>
public class SlaPolicy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationId { get; set; }
    public string Version { get; set; } = "v1";
    public int DurationHours { get; set; } = 48;
    public bool UseWorkingHours { get; set; } = false;
    public string? CustomRulesJson { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
