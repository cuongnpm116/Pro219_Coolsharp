using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.DeleteCartItem;

public class DeleteCartItemCommand : IRequest<Result>
{
    public List<Guid> ProductDetailIds { get; set; }
}
