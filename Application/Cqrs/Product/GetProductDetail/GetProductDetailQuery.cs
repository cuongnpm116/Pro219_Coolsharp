
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetail;

public class GetProductDetailQuery : IRequest<Result>
{
    public Guid ProductId { get; set; }
}

