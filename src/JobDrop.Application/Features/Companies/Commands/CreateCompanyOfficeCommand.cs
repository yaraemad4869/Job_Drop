using JobDrop.Application.Features.Companies.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed record CreateCompanyOfficeCommand(Guid CompanyId, string Nickname, int OfficeType, double Latitude, double Longitude, int GeofenceRadiusMeters, string? MakaniCode) : IRequest<CompanyOfficeDto>;