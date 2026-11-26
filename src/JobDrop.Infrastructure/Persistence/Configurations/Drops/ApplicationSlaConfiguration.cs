using JobDrop.Domain.Entities.Drops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public sealed class ApplicationSlaConfiguration : IEntityTypeConfiguration<ApplicationSla>
{
    public void Configure(EntityTypeBuilder<ApplicationSla> builder)
    {
        builder.ToTable("application_slas");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        
        builder.HasIndex(x => x.ApplicationId).IsUnique();
        builder.HasIndex(x => new { x.Status, x.DueAtUtc });
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
