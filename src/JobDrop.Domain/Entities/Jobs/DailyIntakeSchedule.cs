namespace JobDrop.Domain.Entities.Jobs;

/// <summary>
/// BE-10: Scheduled daily intake limits with effective periods.
/// Supports future dates, time zones, and manual pauses.
/// </summary>
public class DailyIntakeSchedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JobId { get; set; }
    public DateOnly EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
    public int CapLimit { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public bool IsManuallyPaused { get; set; } = false;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid CreatedByUserId { get; set; }
    
    public bool IsActiveOn(DateOnly date)
    {
        if (date < EffectiveFrom) return false;
        if (EffectiveTo.HasValue && date > EffectiveTo.Value) return false;
        return true;
    }
}
