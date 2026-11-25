using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record UpdateCandidateProfileCommand(
    Guid CandidateProfileId,
    string? FullName,
    string? Headline,
    string? Summary,
    string? DropThemeCode) : IRequest<Unit>;

public sealed class UpdateCandidateProfileHandler : IRequestHandler<UpdateCandidateProfileCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCandidateProfileHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(x => x.Id == request.CandidateProfileId, cancellationToken)
            ?? throw new NotFoundException("Candidate profile not found.");

        // Use update methods instead of direct property assignment
        if (request.FullName != null || request.Headline != null)
        {
            profile.UpdateProfile(
                request.FullName ?? profile.FullName,
                request.Headline ?? profile.Headline);
        }

        if (request.Summary != null)
        {
            profile.UpdateSummary(request.Summary);
        }

        if (request.DropThemeCode != null)
        {
            profile.UpdateDropTheme(request.DropThemeCode);
        }

        _context.CandidateProfiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
