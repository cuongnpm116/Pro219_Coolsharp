using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(x => x.Name).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(256);

        builder.Property(c => c.Description).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.Status).IsRequired()
            .HasConversion(v => (byte)v, v => (Status)v);
    }
}
