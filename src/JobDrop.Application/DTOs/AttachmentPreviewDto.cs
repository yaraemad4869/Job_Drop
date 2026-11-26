namespace JobDrop.Application.DTOs;

/// <summary>
/// BE-09: Attachment preview with byte limits.
/// </summary>
public record AttachmentPreviewDto(
    long SelectedAttachmentBytes,
    long MaxAttachmentBytes,
    int SupportingAttachmentCount,
    int MaxSupportingAttachments,
    bool IsValid);
