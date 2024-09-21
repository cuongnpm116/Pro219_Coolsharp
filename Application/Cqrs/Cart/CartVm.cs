namespace Application.Cqrs.Cart;

public class CartVm
{
    public Guid CustomerId { get; set; }
    public List<CartItemVm> ListCart { get; set; }
    public decimal TotalPayment { get; set; }
}
