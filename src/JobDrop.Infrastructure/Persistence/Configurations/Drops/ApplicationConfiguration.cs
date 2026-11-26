using JobDrop.Domain.Entities.Drops;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("applications");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.IdempotencyKey)
               .HasMaxLength(100)
               .IsRequired(false);
        
        builder.HasIndex(e => e.IdempotencyKey)
               .IsUnique()
               .HasFilter("IdempotencyKey IS NOT NULL")
               .HasDatabaseName("UX_Application_IdempotencyKey");
        
        builder.Property(e => e.Status)
               .HasConversion<string>()
               .HasMaxLength(50);
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
