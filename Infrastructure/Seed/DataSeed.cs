using Common.Utilities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;
internal static class DataSeed
{
    internal static void SeedData(this ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(PreconfigRoles);
        builder.Entity<Staff>().HasData(PreconfigStaff);
        builder.Entity<StaffRole>().HasData(PreconfigStaffRoles);
    }

    private static readonly Guid _role1Id = new("0fc1d27c-f6c4-4011-8d3c-4d33b2703369");
    private static readonly Guid _role2Id = new("5f266f3f-bfda-4a21-bd55-c3191249bea5");
    internal static readonly List<Role> PreconfigRoles =
    [
        new()
        {
            Id = _role1Id,
            Code = "admin",
            Name = "Quản trị viên",
        },
        new()
        {
            Id = _role2Id,
            Code = "staff",
            Name = "Nhân viên",
        },
    ];

    private static readonly Guid _staff1Id = new("b48703e5-2bc4-4996-88dd-4369d76fd61d");
    internal static readonly List<Staff> PreconfigStaff =
    [
         new()
         {
             Id = _staff1Id,
             FirstName = "Nguyễn",
             LastName = "Cương",
             DateOfBirth = new DateTime(2003, 11, 16),
             Email = "sohardz01@gmail.com",
             Username = "admin",
             HashedPassword = HashUtility.Hash("12345678"),
         },
    ];

    internal static readonly List<StaffRole> PreconfigStaffRoles =
    [
        new()
        {
            Id = Guid.NewGuid(),
            StaffId = _staff1Id,
            RoleId = _role1Id,
        },
    ];
}
