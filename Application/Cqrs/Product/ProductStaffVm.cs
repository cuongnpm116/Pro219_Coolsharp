

using Domain.Enums;

namespace Application.Cqrs.Product;

public class ProductStaffVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal MinPrice { get; set; }
    public int TotalStock { get; set; }
    public Status Status { get; set; }
    public List<ProductDetailStaffVm> ProductDetails { get; set; }
}
