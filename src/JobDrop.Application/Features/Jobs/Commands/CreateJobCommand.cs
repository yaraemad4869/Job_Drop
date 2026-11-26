using MediatR;

namespace JobDrop.Application.Features.Jobs.Commands;

public sealed record CreateJobCommand(
    Guid CompanyId,
    Guid OfficeId,
    string Title,
    string Description,
    string WorkMode,
    int? SalaryMin,
    int? SalaryMax,
    string SalaryCurrency,
    int DailyIntakeCap
) : IRequest<CreateJobResult>;

public sealed record CreateJobResult(Guid JobId);