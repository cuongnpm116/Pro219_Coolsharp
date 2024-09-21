using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateProductInfo;
internal class UpdateProductInfoCommandHandler
    : IRequestHandler<UpdateProductInfoCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductInfoCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(UpdateProductInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.UpdateProductInfoAsync(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
