using JobDrop.Domain.Entities.ReverseDrops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.ReverseDrops;

public sealed class ReverseDropPoolConfiguration : IEntityTypeConfiguration<ReverseDropPool>
{
    public void Configure(EntityTypeBuilder<ReverseDropPool> builder)
    {
        builder.ToTable("reverse_drop_pools");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => new { x.CompanyId, x.CycleStartUtc });
        
        // Optimistic Concurrency
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
