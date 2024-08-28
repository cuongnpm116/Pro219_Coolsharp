using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(pic => pic.Id);

        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(pic => pic.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pic => pic.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pic => pic.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pic => pic.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
