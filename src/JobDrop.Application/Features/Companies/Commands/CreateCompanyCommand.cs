using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed record CreateCompanyCommand(
    string LegalName,
    string DisplayName,
    string? Industry,
    string? Website
) : IRequest<CreateCompanyResult>;

public sealed record CreateCompanyResult(Guid CompanyId);