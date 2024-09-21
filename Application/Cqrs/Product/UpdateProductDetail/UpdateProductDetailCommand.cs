using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateProductDetail;
public class UpdateProductDetailCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public int Stock { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
}
