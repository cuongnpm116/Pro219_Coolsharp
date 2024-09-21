using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateDetailWithNewImages;
internal class UpdateDetailWithNewImageCommandHandler
    : IRequestHandler<UpdateDetailWithNewImageCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public UpdateDetailWithNewImageCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(UpdateDetailWithNewImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.UpdateDetailWithNewImage(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
