using MediatR;

namespace JobDrop.Application.Features.Admin.Commands;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<bool>;