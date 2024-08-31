namespace CustomerWebApp.Components.Carts.Dtos;

public class AddCartRequest
{
    public Guid ColorId { get; set; }
    public Guid ProductId { get; set; }
    public Guid SizeId { get; set; }
    public int Quantity { get; set; } = 1;
    public Guid CustomerId { get; set; }
}
