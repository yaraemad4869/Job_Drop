using FluentValidation;
using JobDrop.Domain.Common;

namespace JobDrop.Application.Features.Drops.Validators;

/// <summary>
/// BE-09: Validates Drop attachment limits.
/// Enforces independent 10 MB total limit per Drop, separate from the 20 MB profile limit.
/// </summary>
public class DropAttachmentValidator : AbstractValidator<DropAttachmentRequest>
{
    public DropAttachmentValidator()
    {
        // BE-09: Independent 10 MB total attachment limit per Drop
        RuleFor(x => x.TotalAttachmentBytes)
            .LessThanOrEqualTo(DomainConstants.MaxDropAttachmentBytes)
            .WithMessage($"Total attachment size must not exceed {DomainConstants.MaxDropAttachmentBytes / (1024 * 1024)} MB per Drop.");

        // BE-09: Maximum 5 supporting attachments
        RuleFor(x => x.SupportingAttachmentCount)
            .LessThanOrEqualTo(DomainConstants.MaxSupportingAttachmentsPerDrop)
            .WithMessage($"Maximum {DomainConstants.MaxSupportingAttachmentsPerDrop} supporting attachments allowed per Drop.");

        // BE-09: Server-side validation of ownership, redaction, and malware status
        RuleFor(x => x.Attachments)
            .Must(attachments => attachments != null && attachments.All(a => a.IsOwned && a.IsRedacted && a.IsMalwareClean))
            .WithMessage("All attachments must be owned, redacted, and malware-scanned before dispatch.");
            
        RuleFor(x => x.Attachments)
            .NotNull()
            .WithMessage("Attachments list cannot be null.");
    }
}

public class DropAttachmentRequest
{
    public long TotalAttachmentBytes { get; set; }
    public int SupportingAttachmentCount { get; set; }
    public List<AttachmentInfo> Attachments { get; set; } = new();
}

public class AttachmentInfo
{
    public Guid DocumentId { get; set; }
    public long SizeBytes { get; set; }
    public bool IsOwned { get; set; }
    public bool IsRedacted { get; set; }
    public bool IsMalwareClean { get; set; }
}
