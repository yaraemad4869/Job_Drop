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
}