using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Companies.DTOs;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Exceptions;
using JobDrop.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed class CreateCompanyOfficeHandler : IRequestHandler<CreateCompanyOfficeCommand, CompanyOfficeDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CreateCompanyOfficeHandler(IApplicationDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CompanyOfficeDto> Handle(CreateCompanyOfficeCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenAccessException();

        // Verify user is Owner or Admin of the company
        var isAuthorized = await _db.CompanyMembers
            .AnyAsync(m => m.CompanyId == request.CompanyId && m.UserId == userId && 
                          (m.Role == CompanyMemberRole.Owner || m.Role == CompanyMemberRole.Admin), cancellationToken);
        
        if (!isAuthorized) throw new ForbiddenAccessException("Only Company Owners or Admins can add offices.");

        var office = new CompanyOffice
        {
            CompanyId = request.CompanyId,
            Nickname = request.Nickname,
            OfficeType = (OfficeType)request.OfficeType,
            Location = new GeoPoint(request.Latitude, request.Longitude),
            GeofenceRadiusMeters = request.GeofenceRadiusMeters,
            MakaniCode = request.MakaniCode,
            VerificationLevel = VerificationLevel.Unverified,
            LocationVersion = 1
        };

        _db.CompanyOffices.Add(office);
        await _db.SaveChangesAsync(cancellationToken);

        return new CompanyOfficeDto(
            office.Id, office.Nickname, (int)office.OfficeType,
            office.Location.Latitude, office.Location.Longitude,
            office.GeofenceRadiusMeters, (int)office.VerificationLevel);
    }
}