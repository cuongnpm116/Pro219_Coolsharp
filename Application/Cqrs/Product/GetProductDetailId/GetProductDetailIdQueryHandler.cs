using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailId;

internal sealed class GetProductDetailIdQueryHandler : IRequestHandler<GetProductDetailIdQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductDetailIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(GetProductDetailIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductDetailId(request.ProductId,request.ColorId,request.SizeId);
            
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
