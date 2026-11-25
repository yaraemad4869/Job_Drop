using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Jobs;

public sealed class JobAssignment : BaseEntity
{
    public Guid JobId { get; set; }
    public Guid CompanyMemberId { get; set; }
    public bool CanTriage { get; set; } = true;
    public bool CanDispatch { get; set; }

    public Job? Job { get; set; }
    public Companies.CompanyMember? Member { get; set; }
}