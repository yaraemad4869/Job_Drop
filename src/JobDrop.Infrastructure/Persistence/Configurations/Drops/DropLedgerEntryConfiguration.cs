using JobDrop.Domain.Entities.Drops;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public class DropLedgerEntryConfiguration : IEntityTypeConfiguration<DropLedgerEntry>
{
    public void Configure(EntityTypeBuilder<DropLedgerEntry> builder)
    {
        builder.ToTable("drop_ledger_entries");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.IdempotencyKey)
               .HasMaxLength(100)
               .IsRequired(false);
        
        builder.HasIndex(e => e.IdempotencyKey)
               .IsUnique()
               .HasFilter("IdempotencyKey IS NOT NULL")
               .HasDatabaseName("UX_DropLedgerEntry_IdempotencyKey");
        
        builder.Property(e => e.Amount)
               .HasColumnType("numeric(18,4)");
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
