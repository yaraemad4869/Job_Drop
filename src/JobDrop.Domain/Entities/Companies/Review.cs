using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Companies;

public sealed class Review : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public Guid ReviewerUserId { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public Guid TargetId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}