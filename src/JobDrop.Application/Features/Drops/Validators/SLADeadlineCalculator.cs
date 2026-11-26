using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Application.Features.Drops.Validators;

/// <summary>
/// BE-01: Validates and calculates SLA deadlines.
/// Policy: DueAtUtc = DeliveredAtUtc + 48 elapsed hours.
/// Queued applications do not consume the response window (deliveredAtUtc is null).
/// </summary>
public static class SLADeadlineCalculator
{
    /// <summary>
    /// Calculates the SLA deadline based on delivery time.
    /// Returns null if the application is still Queued (not yet delivered).
    /// </summary>
    public static DateTimeOffset? CalculateDueAtUtc(DateTimeOffset? deliveredAtUtc, string policyVersion = "v1")
    {
        // BE-01: Keep dueAtUtc unset while the application is Queued.
        if (!deliveredAtUtc.HasValue)
        {
            return null;
        }

        // Future-proofing for policy versions (e.g., working hours vs. elapsed hours)
        return policyVersion switch
        {
            "v1" => deliveredAtUtc.Value.AddHours(DomainConstants.DefaultSLAHours),
            _ => deliveredAtUtc.Value.AddHours(DomainConstants.DefaultSLAHours) // Fallback to default
        };
    }

    /// <summary>
    /// BE-05: Classifies match score into standardized bands.
    /// High >= 90, Medium >= 80 and < 90, Low < 80.
    /// </summary>
    public static MatchBand ClassifyMatchScore(double score)
    {
        if (score >= DomainConstants.HighMatchThreshold)
            return MatchBand.High;
        
        if (score >= DomainConstants.MediumMatchThreshold)
            return MatchBand.Medium;
            
        return MatchBand.Low;
    }
}
