using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Jobs;

public class JobAssignment : BaseEntity
{
    public Guid JobId { get; private set; }
    public Guid CompanyMemberId { get; private set; }
    public bool CanTriage { get; private set; }
    public bool CanDispatch { get; private set; }

    private JobAssignment() { }

    public JobAssignment(Guid jobId, Guid companyMemberId, bool canTriage, bool canDispatch)
    {
        JobId = jobId;
        CompanyMemberId = companyMemberId;
        CanTriage = canTriage;
        CanDispatch = canDispatch;
    }
}
