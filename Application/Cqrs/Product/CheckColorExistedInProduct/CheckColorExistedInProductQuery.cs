using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.CheckColorExistedInProduct;
public class CheckColorExistedInProductQuery : IRequest<Result<bool>>
{
    public Guid ProductId { get; set; }
    public Guid ColorId { get; set; }
}
