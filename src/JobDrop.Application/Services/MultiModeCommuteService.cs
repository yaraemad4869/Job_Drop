using JobDrop.Domain.Enums;

namespace JobDrop.Application.Services;

/// <summary>
/// BE-13: Calculates commute times for multiple modes with per-mode thresholds.
/// </summary>
public class MultiModeCommuteService
{
    /// <summary>
    /// Determines if a job qualifies based on multi-mode commute policy.
    /// Policy: Job qualifies when ANY selected mode satisfies its threshold.
    /// </summary>
    public bool JobQualifies(Dictionary<CommuteMode, int> modeResults, Dictionary<CommuteMode, int> thresholds)
    {
        foreach (var (mode, actualMinutes) in modeResults)
        {
            if (thresholds.TryGetValue(mode, out var threshold) && actualMinutes <= threshold)
            {
                return true;
            }
        }
        return false;
    }

    public CommuteMode GetPrimaryModeForSorting(Dictionary<CommuteMode, int> modeResults, Dictionary<CommuteMode, int> thresholds)
    {
        // Use the mode with the shortest time among those that satisfy threshold
        CommuteMode? bestMode = null;
        int bestTime = int.MaxValue;

        foreach (var (mode, actualMinutes) in modeResults)
        {
            if (thresholds.TryGetValue(mode, out var threshold) && actualMinutes <= threshold && actualMinutes < bestTime)
            {
                bestMode = mode;
                bestTime = actualMinutes;
            }
        }

        return bestMode ?? CommuteMode.Driving;
    }
}
