using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.ReverseDrops;

public sealed class ReverseDropPool : ConcurrentAuditableEntity
{
    public Guid CompanyId { get; set; }
    public DateTimeOffset CycleStartUtc { get; set; }
    public DateTimeOffset CycleEndUtc { get; set; }
    public int MonthlyRemaining { get; set; }
    public int TopUpRemaining { get; set; }

    public Companies.Company? Company { get; set; }

    public bool TryConsumeCredit()
    {
        if (MonthlyRemaining > 0)
        {
            MonthlyRemaining--;
            return true;
        }
        if (TopUpRemaining > 0)
        {
            TopUpRemaining--;
            return true;
        }
        return false;
    }

    public void RefundCredit()
    {
        MonthlyRemaining++;
    }
}
