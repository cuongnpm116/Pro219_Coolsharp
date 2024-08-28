using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber).IsRequired()
             .HasColumnType("varchar(15)");

        builder.Property(x => x.ProvinceCode).IsRequired()
            .HasColumnType("char(2)");

        builder.Property(x => x.DistrictCode).IsRequired()
            .HasColumnType("char(3)");

        builder.Property(x => x.WardCode).IsRequired()
            .HasColumnType("char(5)");

        builder.Property(x => x.Detail).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.IsDefault).IsRequired();

        builder.HasOne(x => x.Customer)
            .WithMany(y => y.Addresses)
            .HasForeignKey(x => x.CustomerId);
    }
}
