using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed record CreateCompanyOfficeCommand(
    Guid CompanyId,
    string Nickname,
    string OfficeType,
    double Latitude,
    double Longitude,
    int GeofenceRadiusMeters,
    string? TransitAnchorText) : IRequest<Guid>;
