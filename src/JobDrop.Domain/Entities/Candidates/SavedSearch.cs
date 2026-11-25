using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class SavedSearch : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public string SearchName { get; set; } = string.Empty;
    public string FiltersJson { get; set; } = "{}";
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}