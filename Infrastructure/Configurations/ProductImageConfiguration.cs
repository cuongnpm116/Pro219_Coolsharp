using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ProductDetail)
            .WithMany(y => y.ProductImages)
            .HasForeignKey(x => x.ProductDetailId);

        builder.HasOne(x => x.Image)
            .WithMany(y => y.ProductImages)
            .HasForeignKey(x => x.ImageId);
    }
}
