using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(128);

        builder.Property(x => x.Name).IsRequired()
            .HasColumnType("nvarchar")
            .HasMaxLength(256);
    }
}
