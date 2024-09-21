using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.CheckColorExistedInProduct;
internal class CheckColorExistedInProductQueryHandler
    : IRequestHandler<CheckColorExistedInProductQuery, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public CheckColorExistedInProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(CheckColorExistedInProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.CheckColorExistedInProduct(request.ProductId, request.ColorId);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
