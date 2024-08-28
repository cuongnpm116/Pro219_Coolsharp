using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(x => x.Name).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(256);

        builder.Property(x => x.Status).IsRequired();
    }
}
