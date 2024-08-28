using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId).IsRequired();

        builder.HasOne(x => x.Customer)
            .WithOne(y => y.Cart)
            .HasForeignKey<Cart>(x => x.CustomerId);
    }
}
