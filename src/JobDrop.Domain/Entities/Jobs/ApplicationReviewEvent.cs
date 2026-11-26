namespace JobDrop.Domain.Entities.Jobs;

/// <summary>
/// BE-12: Review and comment events with visibility scope.
/// Separates internal hiring-team notes from candidate-facing feedback.
/// </summary>
public class ApplicationReviewEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationId { get; set; }
    public string EventType { get; set; } = string.Empty; // Reviewed, Commented, Feedback
    public Guid AuthorUserId { get; set; }
    public string VisibilityScope { get; set; } = "Internal"; // Internal, Candidate
    public string? Content { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool SatisfiesSLA { get; set; } = false;
    
    public bool IsVisibleToCandidate => VisibilityScope == "Candidate";
}
