using MediatR;

namespace JobDrop.Application.Features.Companies.Commands;

public sealed record InviteMemberCommand(
    Guid CompanyId,
    string Email,
    string Role
) : IRequest<InviteMemberResult>;

public sealed record InviteMemberResult(Guid InvitationId);