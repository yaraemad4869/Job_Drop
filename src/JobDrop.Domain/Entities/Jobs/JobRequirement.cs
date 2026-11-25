using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Jobs;

public sealed class JobRequirement : BaseEntity
{
    public Guid JobId { get; set; }
    public string RequirementType { get; set; } = string.Empty;
    public Guid? CatalogItemId { get; set; }
    public string? ItemName { get; set; }
    public ProficiencyLevel? RequiredLevel { get; set; }
    public bool IsMandatory { get; set; } = true;

    public Job? Job { get; set; }
}