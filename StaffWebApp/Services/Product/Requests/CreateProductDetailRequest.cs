namespace StaffWebApp.Services.Product.Requests;

public class CreateProductDetailRequest
{
    public Guid Id { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }

    public CreateProductDetailRequest(
        Guid id,
        Guid colorId,
        Guid sizeId,
        int stock,
        decimal price,
        decimal originalPrice)
    {
        Id = id;
        ColorId = colorId;
        SizeId = sizeId;
        Stock = stock;
        Price = price;
        OriginalPrice = originalPrice;
    }
}
