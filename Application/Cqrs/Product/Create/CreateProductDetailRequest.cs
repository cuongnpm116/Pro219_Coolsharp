namespace Application.Cqrs.Product.Create;

public class CreateProductDetailRequest
{
    public Guid Id { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
}