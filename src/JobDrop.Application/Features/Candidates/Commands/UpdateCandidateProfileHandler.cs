using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Candidates.DTOs;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed class UpdateCandidateProfileHandler : IRequestHandler<UpdateCandidateProfileCommand, CandidateProfileDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;

    public UpdateCandidateProfileHandler(IApplicationDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CandidateProfileDto> Handle(UpdateCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenAccessException();
        
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Candidates.CandidateProfile), userId);

        if (request.Payload.Headline != null) profile.Headline = request.Payload.Headline;
        if (request.Payload.Summary != null) profile.Summary = request.Payload.Summary;
        if (request.Payload.IsOptedInTalentSearch.HasValue) profile.IsOptedInTalentSearch = request.Payload.IsOptedInTalentSearch.Value;
        if (request.Payload.DropThemeCode != null) profile.DropThemeCode = request.Payload.DropThemeCode;

        // TODO: Recalculate ProfileCompletenessPct based on sub-resources
        profile.ProfileCompletenessPct = 50.0m; // Placeholder

        await _db.SaveChangesAsync(cancellationToken);

        return new CandidateProfileDto(
            profile.Id, profile.Headline, profile.Summary, profile.ProfileCompletenessPct,
            profile.IsOptedInTalentSearch, profile.DropThemeCode);
    }
}