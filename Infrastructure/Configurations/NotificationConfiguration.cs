using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Message).IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.HasOne(x => x.Customer)
           .WithMany(y => y.Notifications)
           .HasForeignKey(x => x.CustomerId);
    }
}
