using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.AddDetailWithNewImages;
internal class AddDetailWithNewImagesCommandHandler
    : IRequestHandler<AddDetailWithNewImagesCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;

    public AddDetailWithNewImagesCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(AddDetailWithNewImagesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _productRepository.AddDetailWithNewImages(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
