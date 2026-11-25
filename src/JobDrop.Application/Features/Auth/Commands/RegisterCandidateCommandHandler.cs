using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed record RegisterCandidateCommand(
    string Email,
    string Password,
    string Handle,
    string FullName,
    string? Headline) : IRequest<AuthResponse>;

public sealed class RegisterCandidateCommandHandler : IRequestHandler<RegisterCandidateCommand, AuthResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokens;
    private readonly IApplicationDbContext _context;

    public RegisterCandidateCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokens,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokens = tokens;
        _context = context;
    }

    public async Task<AuthResponse> Handle(RegisterCandidateCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new DomainRuleException("Email already registered.", "EMAIL_ALREADY_EXISTS");
        }

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            Handle = request.Handle,
            IsActive = true,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DomainRuleException($"User creation failed: {errors}", "USER_CREATION_FAILED");
        }

        await _userManager.AddToRoleAsync(user, "JobSeeker");

        // Use new constructor with userId, fullName, headline
        var candidateProfile = new CandidateProfile(user.Id, request.FullName, request.Headline);

        _context.CandidateProfiles.Add(candidateProfile);
        await _context.SaveChangesAsync(cancellationToken);

        // Create wallet with constructor
        var wallet = new DropWalletBalance(candidateProfile.Id, 5);
        _context.DropWalletBalances.Add(wallet);
        await _context.SaveChangesAsync(cancellationToken);

        var roles = await _userManager.GetRolesAsync(user);
        var tokens = await _tokens.GenerateTokensAsync(user, roles.ToList(), null, cancellationToken);

        return new AuthResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresAtUtc,
            user.Handle,
            user.Email ?? string.Empty,
            roles.ToList());
    }
}
