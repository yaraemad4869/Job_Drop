using JobDrop.Domain.Entities.Companies;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Companies;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
               .HasMaxLength(256)
               .IsRequired();
        
        builder.Property(e => e.FollowUpRate)
               .HasColumnType("numeric(5,2)");
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
