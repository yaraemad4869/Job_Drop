using JobDrop.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Jobs;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ThemeColorHex).HasMaxLength(9);
        builder.Property(x => x.Description).HasMaxLength(8000).IsRequired();
        builder.Property(x => x.SalaryMin).HasPrecision(18, 2);
        builder.Property(x => x.SalaryMax).HasPrecision(18, 2);
        builder.Property(x => x.SalaryCurrency).HasMaxLength(8).HasDefaultValue("USD");
        builder.Property(x => x.SalaryPeriod).HasMaxLength(20).HasDefaultValue("Monthly");
        builder.HasOne(x => x.Company).WithMany(c => c.Jobs).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Office).WithMany().HasForeignKey(x => x.OfficeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.Status, x.PublishedAtUtc });
        builder.HasIndex(x => new { x.CompanyId, x.Status });
    }
}