using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity).IsRequired();

        builder.HasOne(x => x.Cart)
            .WithMany(y => y.CartItems)
            .HasForeignKey(x => x.CartId);

        builder.HasOne(x => x.ProductDetail)
            .WithMany(y => y.CartItems)
            .HasForeignKey(x => x.ProductDetailId);
    }
}
