using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(100);

        builder.Property(x => x.LastName).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(256);

        builder.Property(x => x.EmailAddress).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(324);
        builder.HasIndex(x => x.EmailAddress).IsUnique();

        builder.Property(x => x.Username).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(30);
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.ImageUrl)
            .HasColumnType("varchar")
            .HasMaxLength(512);

        builder.Property(x => x.HashedPassword).IsRequired()
            .HasColumnType("varchar(max)");
    }
}
