using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed record AddOfficeCommand(
    Guid CompanyId,
    string Nickname,
    string OfficeType,
    Guid OperatingAreaId
) : IRequest<AddOfficeResult>;

public sealed record AddOfficeResult(Guid OfficeId);