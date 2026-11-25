using JobDrop.Domain.Enums;

namespace JobDrop.Application.Features.Candidates.DTOs;

public sealed record CandidateProfileDto(
    Guid Id,
    string Headline,
    string Summary,
    decimal ProfileCompletenessPct,
    bool IsOptedInTalentSearch,
    string DropThemeCode);

public sealed record UpdateCandidateProfileRequest(
    string? Headline,
    string? Summary,
    bool? IsOptedInTalentSearch,
    string? DropThemeCode);

public sealed record WorkExperienceDto(Guid Id, string CountryCode, string JobTitle, string EmployerName, DateOnly StartDate, DateOnly? EndDate, string Description);
public sealed record CreateWorkExperienceRequest(string CountryCode, string JobTitle, string EmployerName, DateOnly StartDate, DateOnly? EndDate, string Description);
public sealed record UpdateWorkExperienceRequest(Guid Id, string CountryCode, string JobTitle, string EmployerName, DateOnly StartDate, DateOnly? EndDate, string Description);

public sealed record SkillDto(Guid Id, Guid SkillId, string SkillName, ProficiencyLevel ProficiencyLevel, bool IsVerified);
public sealed record UpdateSkillsRequest(List<SkillUpdateItem> Skills);
public sealed record SkillUpdateItem(Guid SkillId, ProficiencyLevel ProficiencyLevel);

public sealed record StartingPointDto(Guid Id, string Label, AnchorType AnchorType, double Latitude, double Longitude, bool IsActiveOrigin);
public sealed record CreateStartingPointRequest(string Label, int AnchorType, double Latitude, double Longitude, bool IsActiveOrigin);
public sealed record UpdateStartingPointRequest(Guid Id, string Label, AnchorType AnchorType, double Latitude, double Longitude, bool IsActiveOrigin);

public sealed record DocumentDto(Guid Id, DocumentKind Kind, string OriginalFileName, long SizeBytes, ScanStatus ScanStatus);