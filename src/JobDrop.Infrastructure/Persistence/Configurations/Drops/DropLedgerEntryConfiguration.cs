using JobDrop.Domain.Entities.Drops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public sealed class DropLedgerEntryConfiguration : IEntityTypeConfiguration<DropLedgerEntry>
{
    public void Configure(EntityTypeBuilder<DropLedgerEntry> builder)
    {
        builder.ToTable("DropLedgerEntries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ReasonCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.IdempotencyKey).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
        builder.HasIndex(x => new { x.CandidateProfileId, x.CreatedAtUtc });
    }
}