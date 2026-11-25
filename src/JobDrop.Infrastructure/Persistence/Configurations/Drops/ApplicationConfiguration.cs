using JobDrop.Domain.Entities.Drops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DropApplication = JobDrop.Domain.Entities.Drops.Application;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public sealed class ApplicationConfiguration : IEntityTypeConfiguration<DropApplication>
{
    public void Configure(EntityTypeBuilder<DropApplication> builder)
    {
        builder.ToTable("Applications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.CoverNote).HasMaxLength(2000);
        builder.Property(x => x.RejectionReasonCode).HasMaxLength(100);
        builder.Property(x => x.RejectionNote).HasMaxLength(1000);
        builder.Property(x => x.IdempotencyKey).HasMaxLength(200);
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasOne(x => x.Job).WithMany(j => j.Applications).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CandidateProfile).WithMany().HasForeignKey(x => x.CandidateProfileId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.CandidateProfileId, x.JobId }).IsUnique();
        builder.HasIndex(x => new { x.JobId, x.Status, x.DeliveredAtUtc });
    }
}