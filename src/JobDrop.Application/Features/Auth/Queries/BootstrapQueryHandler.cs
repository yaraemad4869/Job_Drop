using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Auth.Queries;

public sealed class BootstrapQueryHandler : IRequestHandler<BootstrapQuery, BootstrapResponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly IApplicationDbContext _db;
    private readonly IDateTimeProvider _time;

    public BootstrapQueryHandler(ICurrentUser currentUser, IApplicationDbContext db, IDateTimeProvider time)
    {
        _currentUser = currentUser;
        _db = db;
        _time = time;
    }

    public async Task<BootstrapResponse> Handle(BootstrapQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenAccessException();

        var user = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Handle, u.Email, u.AvatarCode, u.PreferredLanguage, u.PreferredTheme, u.Currency, u.TimeZoneId, u.CalendarType, u.EmailConfirmed })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("User", userId);

        var memberships = await _db.CompanyMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId && m.IsActive)
            .Join(_db.Companies.AsNoTracking(), m => m.CompanyId, c => c.Id, (m, c) => new CompanyMembershipDto(c.Id, c.DisplayName, (int)m.Role))
            .ToListAsync(cancellationToken);

        var wallet = await _db.CandidateProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Join(_db.DropWalletBalances.AsNoTracking(), p => p.Id, w => w.CandidateProfileId, (p, w) => new WalletBalancesDto(w.DailyRemaining, w.DailyLimit, w.BonusBalance, w.SpecialBalance))
            .FirstOrDefaultAsync(cancellationToken)
            ?? new WalletBalancesDto(0, 0, 0, 0);

        return new BootstrapResponse(
            userId,
            user.Handle,
            user.Email ?? string.Empty,
            (int)user.AvatarCode,
            user.EmailConfirmed,
            memberships.Count > 0 ? "Recruiter" : "JobSeeker",
            memberships,
            wallet,
            0,
            0,
            new UserPreferencesDto((int)user.PreferredLanguage, (int)user.PreferredTheme, user.Currency, user.TimeZoneId, (int)user.CalendarType),
            Array.Empty<string>(),
            _time.UtcNow);
    }
}