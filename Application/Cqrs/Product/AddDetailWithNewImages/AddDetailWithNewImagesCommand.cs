using Application.Cqrs.Product.Create;
using Application.Cqrs.Product.UpdateDetailWithNewImages;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.AddDetailWithNewImages;
public class AddDetailWithNewImagesCommand : IRequest<Result<bool>>
{
    public Guid ProductId { get; set; }
    public UpdateDetailDto Detail { get; set; }
    public IEnumerable<CreateImageRequest> NewImages { get; set; }
    public IEnumerable<CreateProductImageRequest> NewProductImages { get; set; }
}
