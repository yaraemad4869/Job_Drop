using JobDrop.Domain.Common;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.Entities.Drops;

namespace JobDrop.Domain.Entities.Jobs;

public sealed class Job : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Guid? OfficeId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string WorkMode { get; private set; } = string.Empty;
    public string EmploymentType { get; private set; } = string.Empty;
    public string? ThemeColorHex { get; private set; }
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public string? SalaryCurrency { get; private set; }
    public string? SalaryPeriod { get; private set; }
    public string Status { get; private set; } = "Draft";
    public DateTime? PublishedAtUtc { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    // Intake Cap Properties (Master Sprint)
    public int DailyIntakeLimit { get; private set; } = 0;
    public int CurrentApplicantsCount { get; private set; } = 0;
    
    // Navigation Properties
    public Company? Company { get; private set; }
    public CompanyOffice? Office { get; private set; }
    public ICollection<JobDailyIntake> DailyIntakes { get; private set; } = new List<JobDailyIntake>();
    public ICollection<Application> Applications { get; private set; } = new List<Application>();

    private Job() { }

    public Job(Guid companyId, string title, string description, string workMode, string employmentType)
    {
        CompanyId = companyId;
        Title = title;
        Description = description;
        WorkMode = workMode;
        EmploymentType = employmentType;
        IsActive = true;
        Status = "Draft";
    }

    public void SetIntakeLimit(int limit)
    {
        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));
        DailyIntakeLimit = limit;
    }

    public bool TryIncrementIntake()
    {
        if (DailyIntakeLimit > 0 && CurrentApplicantsCount >= DailyIntakeLimit)
            return false;
        
        CurrentApplicantsCount++;
        return true;
    }

    public void DecrementIntake()
    {
        if (CurrentApplicantsCount > 0) CurrentApplicantsCount--;
    }

    public void Publish()
    {
        Status = "Published";
        PublishedAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}