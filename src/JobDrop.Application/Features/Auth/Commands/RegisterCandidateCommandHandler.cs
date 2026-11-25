using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed class RegisterCandidateCommandHandler : IRequestHandler<RegisterCandidateCommand, AuthResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IApplicationDbContext _db;
    private readonly ITokenService _tokens;

    public RegisterCandidateCommandHandler(
        UserManager<AppUser> userManager,
        IApplicationDbContext db,
        ITokenService tokens)
    {
        _userManager = userManager;
        _db = db;
        _tokens = tokens;
    }

    public async Task<AuthResponse> Handle(RegisterCandidateCommand request, CancellationToken cancellationToken)
    {
        var handleNormalized = request.Handle.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(u => u.Handle == handleNormalized, cancellationToken))
            throw new DomainRuleException("Handle is already taken.", "HANDLE_TAKEN");

        var user = new AppUser
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            UserName = request.Email.Trim().ToLowerInvariant(),
            Handle = handleNormalized,
            EmailConfirmed = false,
            LockoutEnabled = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
            throw new DomainRuleException(errors);
        }

        await _userManager.AddToRoleAsync(user, "JobSeeker");

        var profile = new CandidateProfile
        {
            UserId = user.Id,
            Headline = request.FullName ?? string.Empty,
            Summary = string.Empty
        };
        _db.CandidateProfiles.Add(profile);

        var wallet = new DropWalletBalance
        {
            CandidateProfileId = profile.Id,
            DailyRemaining = 5,
            DailyLimit = 5,
            DailyResetAtUtc = DateTimeOffset.UtcNow.Date.AddDays(1),
            BonusBalance = 0,
            SpecialBalance = 0
        };
        _db.DropWalletBalances.Add(wallet);

        await _db.SaveChangesAsync(cancellationToken);

        var roles = await _userManager.GetRolesAsync(user);
        var tokens = await _tokens.GenerateTokensAsync(user, roles, null, cancellationToken);

        return new AuthResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAtUtc,
            user.Handle,
            user.Email,
            roles.ToList());
    }
}