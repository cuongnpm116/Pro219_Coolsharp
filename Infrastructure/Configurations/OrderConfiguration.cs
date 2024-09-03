using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderCode).IsRequired()
            .HasColumnType("varchar(max)");

        builder.Property(x => x.Note).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.TotalPrice).IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.ShipAddress).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.ShipAddressDetail).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.PhoneNumber).IsRequired()
            .HasColumnType("varchar(15)");

        builder.HasIndex(x => x.CreatedOn);

        builder.HasOne(x => x.Customer)
            .WithMany(y => y.Orders)
            .HasForeignKey(x => x.CustomerId);

        builder.HasOne(x => x.Staff)
            .WithMany(y => y.Orders)
            .HasForeignKey(x => x.StaffId);
        builder.HasOne(x => x.Voucher)
            .WithMany(y => y.Orders)
            .HasForeignKey(x => x.VoucherId);
    }
}
