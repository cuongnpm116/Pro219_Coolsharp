

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductImage;

public class GetProductImageQuery : IRequest<Result>
{
    public Guid ProductId { get; set; }
}
