using MediatR;

namespace JobDrop.Application.Features.Jobs.Commands;

public record CreateJobCommand : IRequest<Guid>
{
    // Core fields
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid CompanyId { get; init; }
    public string? Location { get; init; }
    
    // Fields required by CreateJobCommandValidator
    public Guid OfficeId { get; init; }
    public int DailyIntakeCap { get; init; } = 20;
    public decimal? SalaryMin { get; init; }
    public decimal? SalaryMax { get; init; }
    
    // Optional fields
    public string? Currency { get; init; }
    public string? EmploymentType { get; init; }
    public string? ExperienceLevel { get; init; }
}
