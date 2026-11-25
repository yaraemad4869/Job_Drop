using System.ComponentModel.DataAnnotations.Schema;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class WorkExperience : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;

    [NotMapped]
    public bool IsCurrent => !EndDate.HasValue;
}