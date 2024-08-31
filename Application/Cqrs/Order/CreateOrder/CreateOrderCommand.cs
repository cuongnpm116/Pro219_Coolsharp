using Application.Cqrs.Cart;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.CreateOrder;

public class CreateOrderCommand : IRequest<Result>
{
    public Guid CustomerId { get; set; }
    public IReadOnlyList<CartItemVm> Carts { get; set; }
    public decimal TotalPrice { get; set; }
    public string ShipAddress { get; set; }
    public string ShipAddressDetail { get; set; }
    public string PhoneNumber { get; set; }
    public string? Note { get; set; }
}
