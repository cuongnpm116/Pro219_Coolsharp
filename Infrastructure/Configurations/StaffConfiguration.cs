using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(x => x.LastName).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(70);

        builder.Property(x => x.Email).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(324);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Username).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(30);
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.HashedPassword).IsRequired()
            .HasColumnType("varchar(max)");

        builder.Property(x => x.ImageUrl)
            .HasColumnType("varchar(max)");
    }
}
