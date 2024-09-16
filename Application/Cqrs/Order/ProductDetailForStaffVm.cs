namespace Application.Cqrs.Order;

public class ProductDetailForStaffVm
{
    public Guid ProductDetailId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int SizeNumber { get; set; }
    public string ColorName { get; set; }

}
