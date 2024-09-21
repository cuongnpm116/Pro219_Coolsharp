namespace StaffWebApp.Services.Product.Requests.Update;

public class UpdateProductDetailRequest
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public int Stock { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }

    public UpdateProductDetailRequest()
    {
    }

    public UpdateProductDetailRequest(
        Guid id,
        decimal price,
        decimal originalPrice,
        int stock,
        Guid colorId,
        Guid sizeId)
    {
        Id = id;
        Price = price;
        OriginalPrice = originalPrice;
        Stock = stock;
        ColorId = colorId;
        SizeId = sizeId;
    }
}
