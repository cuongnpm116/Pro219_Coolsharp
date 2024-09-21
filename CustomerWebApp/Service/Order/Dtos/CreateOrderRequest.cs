using CustomerWebApp.Service.Cart.ViewModel;

namespace CustomerWebApp.Service.Order.Dtos;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public Guid? VoucherId { get; set; }
    public IReadOnlyList<CartItemVm> Carts { get; set; }
    public decimal TotalPrice { get; set; }
    public string ShipAddress { get; set; }
    public string ShipAddressDetail { get; set; }
    public string PhoneNumber { get; set; }
    public string? Note { get; set; }
}
