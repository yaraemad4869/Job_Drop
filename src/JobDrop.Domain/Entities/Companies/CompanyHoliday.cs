using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyHoliday : BaseEntity
{
    public Guid CompanyId { get; set; }
    public DateOnly HolidayDate { get; set; }
    public string Name { get; set; } = string.Empty;
}