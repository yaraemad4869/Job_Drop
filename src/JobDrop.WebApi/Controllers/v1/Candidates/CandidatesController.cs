using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.Projections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.WebApi.Controllers.v1.Candidates;

[ApiController]
[Route("api/v1/candidates")]
public sealed class CandidatesController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public CandidatesController(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get the blind screening dossier for a candidate (anonymized profile for companies).
    /// Hides PII (name, photo, email, phone) until interview stage.
    /// </summary>
    [HttpGet("{candidateProfileId:guid}/blind-dossier")]
    [Authorize(Roles = "Company,Recruiter")]
    [ProducesResponseType(typeof(BlindScreeningDossierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlindDossier(
        [FromRoute] Guid candidateProfileId,
        CancellationToken cancellationToken)
    {
        var profile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(p => p.Id == candidateProfileId, cancellationToken);

        if (profile == null)
        {
            return NotFound(new { error = "Candidate profile not found.", code = "NOT_FOUND" });
        }

        var dossier = profile.ToBlindDossier();
        return Ok(dossier);
    }
}
