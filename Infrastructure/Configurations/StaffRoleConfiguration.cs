using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<StaffRole>
{
    public void Configure(EntityTypeBuilder<StaffRole> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Staff)
            .WithMany(y => y.StaffRoles)
            .HasForeignKey(x => x.StaffId);

        builder.HasOne(x => x.Role)
            .WithMany(y => y.StaffRoles)
            .HasForeignKey(x => x.RoleId);
    }
}
