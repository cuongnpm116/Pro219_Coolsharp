using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailPrice;

public class GetProductDetailPriceQuery : IRequest<Result>
{
    public Guid ProductId { get; set; }
    public Guid SizeId { get; set; }
    public Guid ColorId { get; set; }
}
