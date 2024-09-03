using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

internal sealed class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(100);

        builder.Property(x => x.VoucherCode).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(256);
        builder.Property(pd => pd.DiscountCondition).IsRequired()
            .HasPrecision(18, 2);
        builder.Property(pd => pd.Stock).IsRequired();
    }
}

