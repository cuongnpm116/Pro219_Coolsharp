

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailStock;

internal sealed class GetProductDetailStockQueryHandler : IRequestHandler<GetProductDetailStockQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductDetailStockQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(GetProductDetailStockQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductDetailStock(request.ProductId, request.ColorId, request.SizeId);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
