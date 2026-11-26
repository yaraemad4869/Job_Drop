using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Geography;

/// <summary>
/// BE-13: Per-mode commute results with individual thresholds.
/// </summary>
public class MultiModeCommuteResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OriginPointId { get; set; }
    public Guid DestinationPointId { get; set; }
    public CommuteMode Mode { get; set; }
    public int DurationMinutes { get; set; }
    public double DistanceKm { get; set; }
    public bool SatisfiesThreshold { get; set; }
    public int ThresholdMinutes { get; set; }
    public DateTimeOffset CalculatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string? RoutingProviderResponse { get; set; }
}
