using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.AddCart;

public class AddCartCommand : IRequest<Result>
{
    public Guid ColorId { get; set; }
    public Guid ProductId { get; set; }
    public Guid SizeId { get; set; }
    public int Quantity { get; set; }
    public Guid CustomerId { get; set; }
}
