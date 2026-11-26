using JobDrop.Domain.Entities.Drops;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public class ApplicationSlaConfiguration : IEntityTypeConfiguration<ApplicationSla>
{
    public void Configure(EntityTypeBuilder<ApplicationSla> builder)
    {
        builder.ToTable("application_slas");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Status)
               .HasConversion<string>()
               .HasMaxLength(50);
        
        builder.HasIndex(e => e.DueAtUtc)
               .HasFilter("Status = 'Pending'")
               .HasDatabaseName("IX_ApplicationSla_DueAtUtc_Pending");
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
