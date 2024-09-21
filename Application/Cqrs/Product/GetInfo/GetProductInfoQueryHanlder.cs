using Application.IRepositories;
using MediatR;

namespace Application.Cqrs.Product.GetInfo;
internal class GetProductInfoQueryHanlder
    : IRequestHandler<GetProductInfoQuery, ProductInfoDto>
{
    private readonly IProductRepository _productRepo;

    public GetProductInfoQueryHanlder(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<ProductInfoDto> Handle(GetProductInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepo.GetProductInfo(request.ProductId);
            return result;
        }
        catch (Exception)
        {
            return new();
        }
    }
}
