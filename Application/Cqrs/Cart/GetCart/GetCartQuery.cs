

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.GetCart;

public class GetCartQuery : IRequest<Result>
{
    public Guid CustomerId { get; set; }
}
