using JobDrop.Domain.Enums;

namespace JobDrop.Application.DTOs;

/// <summary>
/// BE-05: Unified match band DTO with scoring version.
/// </summary>
public record MatchBandDto(
    double MatchScore,
    MatchBand MatchBand,
    string ScoringVersion,
    string? ScoreBreakdownJson = null);
