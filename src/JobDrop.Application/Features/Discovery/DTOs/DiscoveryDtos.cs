namespace JobDrop.Application.Features.Discovery.DTOs;

public sealed record SpatialSearchCommand(
    List<string>? TargetRoles,
    List<Guid>? SkillIds,
    List<int>? WorkModes,
    string? OriginType, // "StartingPoint", "Neighborhood", "CustomPlace"
    Guid? OriginId,
    double? CustomLatitude,
    double? CustomLongitude,
    int? CommuteMode, // 0=ScheduledTransit, 1=Car, 2=Walking, 3=Micromobility
    int? MaxDurationMinutes,
    int? RadiusKm,
    decimal? SalaryMin,
    decimal? SalaryMax,
    bool VerifiedCompaniesOnly,
    bool ShowBeyondCommuteLimit,
    string Sort = "MatchDesc");

public sealed record JobCardDto(
    Guid JobId,
    string Title,
    string CompanyName,
    int VerificationLevel,
    decimal? MatchScore,
    int? CommuteDurationMinutes,
    bool BeyondTime,
    bool BeyondRadius);

public sealed record SpatialSearchResult(
    string SearchContextId,
    List<JobCardDto> Jobs,
    int TotalCount,
    string? NextCursor);