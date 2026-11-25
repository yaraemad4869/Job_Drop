using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyCalendar : BaseEntity
{
    public Guid CompanyId { get; set; }
    public string WorkingDaysJson { get; set; } = "[1,2,3,4,5]";
    public int WorkStartMinute { get; set; } = 540;
    public int WorkEndMinute { get; set; } = 1020;
    public string TimeZoneId { get; set; } = "Africa/Cairo";

    public Company? Company { get; set; }
}