using MediatR;

namespace JobDrop.Application.Features.Jobs.Commands;

public record PublishJobCommand(Guid JobId) : IRequest<bool>;
