using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.UpdateCart;

public class UpdateCartCommand : IRequest<Result>
{
    public Guid CartId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
}
