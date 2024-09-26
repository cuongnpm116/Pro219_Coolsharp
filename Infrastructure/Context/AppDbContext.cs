using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Seed;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;
public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly IHubContext<ShopHub> _hubContext;
    public AppDbContext()
    {

    }
    public AppDbContext(DbContextOptions<AppDbContext> options, IHubContext<ShopHub> hubContext) : base(options)
    {
        _hubContext = hubContext;
    }
    private const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Pro219_eShop;Trusted_Connection=True;TrustServerCertificate=true;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.SeedData();
    }

    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<StaffRole> StaffRoles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Lấy tất cả các thực thể đang bị thay đổi
        var changedEntries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        // Gọi SaveChangesAsync của base để lưu thay đổi vào cơ sở dữ liệu
        int result = await base.SaveChangesAsync(cancellationToken);

        // Sau khi thay đổi được lưu, gửi thông báo SignalR cho các client
        await _hubContext.Clients.All.SendAsync("UpdateDatabase", "Changed");
        

        return result;
    }
}
