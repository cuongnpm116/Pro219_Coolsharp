using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateProductDetail;
internal class UpdateProductDetailCommandHandler
    : IRequestHandler<UpdateProductDetailCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductDetailCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(UpdateProductDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.UpdateProductDetail(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
