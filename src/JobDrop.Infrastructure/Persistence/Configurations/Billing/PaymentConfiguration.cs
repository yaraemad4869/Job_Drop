using JobDrop.Domain.Entities.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Billing;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.MerchantReference).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ProviderOrderId).HasMaxLength(200);
        builder.Property(x => x.ProviderIntentionId).HasMaxLength(200);
        builder.Property(x => x.ProviderTransactionId).HasMaxLength(200);
        builder.Property(x => x.Currency).HasMaxLength(8).IsRequired();
        builder.Property(x => x.FailureReason).HasMaxLength(1000);
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasIndex(x => x.MerchantReference).IsUnique();
        builder.HasIndex(x => x.ProviderTransactionId).IsUnique()
            .HasFilter(@"""ProviderTransactionId"" IS NOT NULL");
    }
}