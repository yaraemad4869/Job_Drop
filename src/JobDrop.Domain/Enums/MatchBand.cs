namespace JobDrop.Domain.Enums;

/// <summary>
/// Match score classification bands (BE-05).
/// Boundaries: High >= 90, Medium >= 80 and less than 90, Low less than 80.
/// </summary>
public enum MatchBand
{
    Low = 0,
    Medium = 1,
    High = 2
}
