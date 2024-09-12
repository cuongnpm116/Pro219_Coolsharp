using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.Create;
internal class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Result>
{
    private IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _productRepository.CreateProductAsync(request);
            return Result<bool>.Success(success);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
