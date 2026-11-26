using MediatR;

namespace JobDrop.Application.Features.Jobs.Commands;

public sealed record PublishJobCommand(Guid JobId) : IRequest<bool>;