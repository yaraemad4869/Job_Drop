using JobDrop.Domain.Enums;

namespace JobDrop.Application.DTOs;

/// <summary>
/// BE-11: Complete talent search contract with explicit filters.
/// </summary>
public class TalentSearchRequest
{
    public Guid? RequisitionId { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Qualifications { get; set; }
    public int? MinExperienceYears { get; set; }
    public int? MaxExperienceYears { get; set; }
    public List<SkillRequirement>? RequiredSkills { get; set; }
    public bool? AvailableImmediately { get; set; }
    public double? MaxDistanceKm { get; set; }
    public Dictionary<CommuteMode, int>? MaxCommuteMinutes { get; set; }
    public double? MinMatchScore { get; set; }
    public string SortBy { get; set; } = "MatchScore";
    public string SortDirection { get; set; } = "Descending";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class SkillRequirement
{
    public string SkillName { get; set; } = string.Empty;
    public int MinProficiencyLevel { get; set; }
    public bool IsMandatory { get; set; }
}
