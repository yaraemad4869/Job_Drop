using System.ComponentModel.DataAnnotations.Schema;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Jobs;

public sealed class JobDailyIntake : ConcurrentAuditableEntity
{
    public Guid JobId { get; set; }
    public DateOnly MarketDate { get; set; }
    public int DeliveredCount { get; set; }
    public int CapLimit { get; set; }

    public Job? Job { get; set; }

    [NotMapped]
    public int Remaining => Math.Max(0, CapLimit - DeliveredCount);

    [NotMapped]
    public bool IsFull => DeliveredCount >= CapLimit;

    public void IncrementDelivered()
    {
        DeliveredCount++;
    }

    /// <summary>
    /// Factory method for test data seeding.
    /// </summary>
    public static JobDailyIntake CreateForTest(Guid jobId, DateOnly marketDate, int deliveredCount = 0)
    {
        var intake = new JobDailyIntake();
        intake.JobId = jobId;
        intake.MarketDate = marketDate;
        intake.DeliveredCount = deliveredCount;
        return intake;
    }
}
