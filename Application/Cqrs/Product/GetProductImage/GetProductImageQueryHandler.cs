using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductImage;

internal sealed class GetProductImageQueryHandler : IRequestHandler<GetProductImageQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductImageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(1, cancellationToken);
            var result = _productRepository.GetDetailImage(request.ProductId);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }


}
