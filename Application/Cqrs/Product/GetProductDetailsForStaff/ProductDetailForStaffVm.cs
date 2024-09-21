using Application.Cqrs.Color.GetForSelect;
using Application.Cqrs.Size.GetForSelect;

namespace Application.Cqrs.Product.GetProductDetailsForStaff;
public class ProductDetailForStaffVm
{
    public Guid Id { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ColorForSelectVm Color { get; set; }
    public SizeForSelectVm Size { get; set; }
}
