namespace Domain.Entities;
public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }

    public virtual Cart? Cart { get; set; }
    public virtual ProductDetail? ProductDetail { get; set; }
}
