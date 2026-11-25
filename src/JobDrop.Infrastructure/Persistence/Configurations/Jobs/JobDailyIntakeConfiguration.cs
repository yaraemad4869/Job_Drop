using JobDrop.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Jobs;

public sealed class JobDailyIntakeConfiguration : IEntityTypeConfiguration<JobDailyIntake>
{
    public void Configure(EntityTypeBuilder<JobDailyIntake> builder)
    {
        builder.ToTable("JobDailyIntakes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.Ignore(x => x.Remaining);
        builder.Ignore(x => x.IsFull);
        builder.HasOne(x => x.Job).WithMany(j => j.DailyIntakes).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.JobId, x.MarketDate }).IsUnique();
    }
}