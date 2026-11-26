using JobDrop.Domain.Entities.Drops;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Drops;

public class DropWalletBalanceConfiguration : IEntityTypeConfiguration<DropWalletBalance>
{
    public void Configure(EntityTypeBuilder<DropWalletBalance> builder)
    {
        builder.ToTable("drop_wallet_balances");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.DailyRemaining)
               .HasColumnType("numeric(18,4)");
        
        builder.Property(e => e.BonusBalance)
               .HasColumnType("numeric(18,4)");
        
        builder.Property(e => e.SpecialBalance)
               .HasColumnType("numeric(18,4)");
        
        builder.ToTable(t => t.HasCheckConstraint("CK_DropWalletBalance_DailyRemaining_NonNegative", "DailyRemaining >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_DropWalletBalance_BonusBalance_NonNegative", "BonusBalance >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_DropWalletBalance_SpecialBalance_NonNegative", "SpecialBalance >= 0"));
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
