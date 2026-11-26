using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Domain.Enums;
using MediatR;

namespace JobDrop.Application.Features.Exports.Handlers;

public sealed class CreateExportJobCommandHandler
    : IRequestHandler<Commands.CreateExportJobCommand, Commands.CreateExportJobResult>
{
    private readonly IApplicationDbContext _db;

    public CreateExportJobCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<Commands.CreateExportJobResult> Handle(
        Commands.CreateExportJobCommand request,
        CancellationToken cancellationToken)
    {
        var exportJob = new ExportJob();
        exportJob.UserId = Guid.Empty; // TODO: From ICurrentUser
        // Fixed: Parse string to ExportKind enum
        exportJob.Kind = Enum.TryParse<ExportKind>(request.Kind, true, out var kind) 
            ? kind 
            : ExportKind.DropsCsv;
        exportJob.Status = ExportJobStatus.Queued;

        _db.ExportJobs.Add(exportJob);
        await _db.SaveChangesAsync(cancellationToken);

        return new Commands.CreateExportJobResult(exportJob.Id);
    }
}