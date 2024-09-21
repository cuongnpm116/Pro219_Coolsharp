using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.AddDetail;
internal class AddDetailCommandHandler
    : IRequestHandler<AddDetailCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public AddDetailCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(AddDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.AddDetailAsync(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
