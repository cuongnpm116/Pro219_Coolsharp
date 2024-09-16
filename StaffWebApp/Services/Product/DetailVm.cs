using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Services.Product;

public class DetailVm
{
    public Guid Id { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ColorForSelectVm Color { get; set; } = new();
    public SizeForSelectVm Size { get; set; } = new();
}
