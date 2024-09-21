

namespace Application.Cqrs.Product;

public class ProductCustomerAppVm
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}
