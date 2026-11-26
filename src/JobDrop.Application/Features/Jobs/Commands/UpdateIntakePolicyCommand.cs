using MediatR;

namespace JobDrop.Application.Features.Jobs.Commands;

public sealed record UpdateIntakePolicyCommand(Guid JobId, int NewDailyCap) : IRequest<bool>;