using Domain.Primitives;
using MediatR;


namespace Application.Cqrs.Product.GetProductDetailStock;

public class GetProductDetailStockQuery : IRequest<Result>
{
    public Guid ProductId { get; set; }
    public Guid SizeId { get; set; }
    public Guid ColorId { get; set; }
}
