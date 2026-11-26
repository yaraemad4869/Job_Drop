using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Services;

/// <summary>
/// BE-16: Provides dossier preview without creating application or debiting wallet.
/// Represents current profile version, no side effects.
/// </summary>
public class DossierPreviewService
{
    private readonly IApplicationDbContext _context;
    private readonly NeutralAvatarService _avatarService;

    public DossierPreviewService(IApplicationDbContext context, NeutralAvatarService avatarService)
    {
        _context = context;
        _avatarService = avatarService;
    }

    /// <summary>
    /// Gets a dossier preview for a candidate.
    /// BE-16: Does NOT create application, debit wallet, or share profile.
    /// </summary>
    public async Task<DossierPreviewDto?> GetPreviewAsync(
        Guid candidateId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(c => c.Id == candidateId, cancellationToken);

        if (profile == null)
        {
            return null;
        }

        // BE-16: Preview without side effects
        return new DossierPreviewDto(
            CandidateId: profile.Id,
            CandidateCode: $"C-{profile.Id.ToString().Substring(0, 8).ToUpper()}", // BE-03: Code only
            Headline: profile.Headline,
            ExperienceYears: profile.ExperienceYears,
            Skills: new List<string>(), // Would be loaded from profile.Skills
            Qualifications: new List<string>(), // Would be loaded from profile.Qualifications
            Attachments: new List<AttachmentPreviewDto>(), // Would be loaded from profile.Documents
            AvatarUrl: _avatarService.GetNeutralAvatar(profile.Id), // BE-04: Neutral avatar
            IsPreview: true // BE-16: Mark as preview
        );
    }
}
