using JobDrop.Domain.Enums;

namespace JobDrop.Application.Features.Companies.DTOs;

public sealed record CompanyDto(Guid Id, string DisplayName, string Industry, int VerificationLevel, bool IsActive);
public sealed record UpdateCompanyRequest(string? DisplayName, string? Industry, string? Website, string? Description);

public sealed record CompanyOfficeDto(Guid Id, string Nickname, int OfficeType, double Latitude, double Longitude, int GeofenceRadiusMeters, int VerificationLevel);
public sealed record CreateCompanyOfficeRequest(string Nickname, int OfficeType, double Latitude, double Longitude, int GeofenceRadiusMeters, string? MakaniCode);
public sealed record UpdateCompanyOfficeRequest(Guid Id, string Nickname, int OfficeType, double Latitude, double Longitude, int GeofenceRadiusMeters, string? MakaniCode);

public sealed record CompanyMemberDto(Guid Id, Guid UserId, string Handle, int Role, bool IsActive);
public sealed record InviteCompanyMemberRequest(string Email, int Role);