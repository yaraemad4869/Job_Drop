using MediatR;

namespace JobDrop.Application.Features.Exports.Commands;

public sealed record CreateExportJobCommand(string Kind) : IRequest<CreateExportJobResult>;

public sealed record CreateExportJobResult(Guid JobId);