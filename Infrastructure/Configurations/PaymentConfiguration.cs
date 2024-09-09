using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.StatusCode).IsRequired()
            .HasColumnType("varchar(128)");

        builder.Property(x => x.Status)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.HasOne(x => x.Customer)
            .WithMany(y => y.Payments)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Order)
             .WithOne(y => y.Payment) 
             .HasForeignKey<Payment>(x => x.OrderId)
             .OnDelete(DeleteBehavior.Cascade); 
    }
}

