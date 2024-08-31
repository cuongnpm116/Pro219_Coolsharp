

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.DeleteCartItem;

internal sealed class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand, Result>
{
    private readonly ICartRepository _cartRepository;
    public DeleteCartItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cartRepository.DeleteCartItem(request.ProductDetailIds);
            return result;
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);

        }
    }
}
