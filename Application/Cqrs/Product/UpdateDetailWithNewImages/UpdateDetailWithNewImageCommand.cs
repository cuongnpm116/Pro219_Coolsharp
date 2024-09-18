using Application.Cqrs.Product.Create;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateDetailWithNewImages;
public class UpdateDetailWithNewImageCommand : IRequest<Result<bool>>
{
    public UpdateDetailDto Detail { get; set; }
    public IEnumerable<CreateImageRequest> NewImages { get; set; }
    public IEnumerable<CreateProductImageRequest> NewProductImages { get; set; }
}
