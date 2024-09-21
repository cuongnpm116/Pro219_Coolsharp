namespace StaffWebApp.Services.Product.Dtos;
public class ProductDetailDto
{
    public Guid Id { get; private set; }
    public Guid ColorId { get; private set; }
    public Guid SizeId { get; private set; }
    public int Stock { get; private set; }
    public decimal Price { get; private set; }
    public decimal OriginalPrice { get; private set; }

    public ProductDetailDto(
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
