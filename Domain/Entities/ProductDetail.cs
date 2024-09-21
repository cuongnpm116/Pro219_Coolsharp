namespace Domain.Entities;
public class ProductDetail
{
    public ProductDetail()
    {
    }

    public ProductDetail(
        Guid id,
        Guid productId,
        Guid sizeId,
        Guid colorId,
        int stock,
        decimal price,
        decimal originalPrice)
    {
        Id = id;
        ProductId = productId;
        SizeId = sizeId;
        ColorId = colorId;
        Stock = stock;
        SalePrice = price;
        OriginalPrice = originalPrice;
    }

    public Guid Id { get; set; }
    public int Stock { get; set; }
    public decimal SalePrice { get; set; }
    public decimal OriginalPrice { get; set; }
    public Guid ProductId { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }

    public virtual Product? Product { get; set; }
    public virtual Color? Color { get; set; }
    public virtual Size? Size { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    public virtual ICollection<CartItem>? CartItems { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; }
}
