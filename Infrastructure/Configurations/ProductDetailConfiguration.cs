using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Stock).IsRequired();

        builder.Property(pd => pd.OriginalPrice).IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pd => pd.SalePrice).IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.ProductDetails)
            .HasForeignKey(pd => pd.ProductId);

        builder.HasOne(pd => pd.Color)
            .WithMany(c => c.ProductDetails)
            .HasForeignKey(pd => pd.ColorId);

        builder.HasOne(pd => pd.Size)
            .WithMany(s => s.ProductDetails)
            .HasForeignKey(pd => pd.SizeId);
    }
}
