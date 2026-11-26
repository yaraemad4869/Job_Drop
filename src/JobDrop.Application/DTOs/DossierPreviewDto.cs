namespace JobDrop.Application.DTOs;

/// <summary>
/// BE-16: Dossier preview without creating application or debiting wallet.
/// </summary>
public record DossierPreviewDto(
    Guid CandidateId,
    string CandidateCode,
    string? Headline,
    int ExperienceYears,
    List<string> Skills,
    List<string> Qualifications,
    List<AttachmentPreviewDto> Attachments,
    string AvatarUrl,
    bool IsPreview);
