using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.Create;
public class CreateProductCommand : IRequest<Result>
{
    public string Name { get; set; }
    public IEnumerable<Guid> CategoryIds { get; set; }
    public IEnumerable<CreateImageRequest> Images { get; set; }
    public IEnumerable<CreateProductDetailRequest> Details { get; set; }
    public IEnumerable<CreateProductImageRequest> DetailImages { get; set; }
}
