using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Candidates.Handlers;

public sealed class UpdateCandidateProfileHandler : IRequestHandler<UpdateCandidateProfileCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateCandidateProfileHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.CandidateProfiles
            .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

        if (profile == null)
            return false;

        // Strictly update only if the new value is not null or whitespace
        if (!string.IsNullOrWhiteSpace(request.Headline))
        {
            profile.UpdateHeadline(request.Headline);
        }

        if (!string.IsNullOrWhiteSpace(request.Summary))
        {
            profile.UpdateSummary(request.Summary);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}