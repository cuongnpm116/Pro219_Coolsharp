namespace Domain.Entities;
public class ProductDetail
{
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
