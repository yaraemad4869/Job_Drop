using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Companies;
using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed class CreateCompanyOfficeHandler : IRequestHandler<CreateCompanyOfficeCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyOfficeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCompanyOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = new CompanyOffice(
            request.CompanyId,
            request.Nickname,
            request.OfficeType,
            request.Latitude,
            request.Longitude,
            request.GeofenceRadiusMeters);

        _context.CompanyOffices.Add(office);
        await _context.SaveChangesAsync(cancellationToken);

        return office.Id;
    }
}
