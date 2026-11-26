using JobDrop.Domain.Entities.Jobs;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Jobs;

public class JobDailyIntakeConfiguration : IEntityTypeConfiguration<JobDailyIntake>
{
    public void Configure(EntityTypeBuilder<JobDailyIntake> builder)
    {
        builder.ToTable("job_daily_intakes");
        
        builder.HasKey(e => e.Id);
        
        builder.ToTable(t => t.HasCheckConstraint("CK_JobDailyIntake_DeliveredCount_Lte_Cap", "DeliveredCount <= CapLimit"));
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
