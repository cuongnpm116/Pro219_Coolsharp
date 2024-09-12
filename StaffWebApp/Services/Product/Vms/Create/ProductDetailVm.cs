using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Services.Product.Vms.Create;
public class ProductDetailVm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public ColorForSelectVm Color { get; set; }
    public SizeForSelectVm Size { get; set; }
}
