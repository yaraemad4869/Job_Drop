using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.Entities.Drops;

namespace JobDrop.Domain.Entities.Jobs;

public sealed class Job : ConcurrentAuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid OfficeId { get; set; }
    public Guid OwnerUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ThemeColorHex { get; set; }
    public string Description { get; set; } = string.Empty;
    public WorkMode WorkMode { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryCurrency { get; set; } = "USD";
    public string SalaryPeriod { get; set; } = "Monthly";
    public bool IsSalaryNegotiable { get; set; }
    public int DailyIntakeCap { get; set; } = 10;
    public JobStatus Status { get; set; } = JobStatus.Draft;
    public DateTimeOffset? PublishedAtUtc { get; set; }
    public DateTimeOffset? ClosedAtUtc { get; set; }

    public Companies.Company? Company { get; set; }
    public Companies.CompanyOffice? Office { get; set; }
    public ICollection<JobAssignment> Assignments { get; set; } = new List<JobAssignment>();
    public ICollection<JobRequirement> Requirements { get; set; } = new List<JobRequirement>();
    public ICollection<JobDailyIntake> DailyIntakes { get; set; } = new List<JobDailyIntake>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}