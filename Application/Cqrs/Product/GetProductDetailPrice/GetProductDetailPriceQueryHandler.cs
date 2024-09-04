
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailPrice;

internal sealed class GetProductDetailPriceQueryHandler : IRequestHandler<GetProductDetailPriceQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductDetailPriceQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(GetProductDetailPriceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductDetailPrice(request.ProductId,request.ColorId, request.SizeId);
            
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
