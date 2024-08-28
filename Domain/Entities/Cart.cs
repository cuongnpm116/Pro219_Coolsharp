namespace Domain.Entities;
public class Cart
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual ICollection<CartItem>? CartItems { get; set; }
}
