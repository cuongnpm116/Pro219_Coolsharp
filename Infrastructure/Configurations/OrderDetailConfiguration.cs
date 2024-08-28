using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Order)
            .WithMany(y => y.OrderDetails)
            .HasForeignKey(x => x.OrderId);

        builder.HasOne(x => x.ProductDetail)
            .WithMany(y => y.OrderDetails)
            .HasForeignKey(x => x.ProductDetailId);
    }
}
