using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Compliance;

/// <summary>
/// BE-03: Tracks redaction status for each document.
/// Never falls back to original document when redaction fails.
/// </summary>
public class RedactionRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; set; }
    public RedactionStatus Status { get; set; } = RedactionStatus.Pending;
    public string? FailureReason { get; set; }
    public Guid? RedactedDocumentId { get; set; }
    public DateTimeOffset? CompletedAtUtc { get; set; }
    public string ProcessorVersion { get; set; } = string.Empty;
    
    public bool IsReadyForBlindReview => 
        Status == RedactionStatus.Completed && RedactedDocumentId.HasValue;
}
