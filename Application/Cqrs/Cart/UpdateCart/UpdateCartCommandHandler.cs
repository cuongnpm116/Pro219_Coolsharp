using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.UpdateCart;

internal sealed class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, Result>
{
    private readonly ICartRepository _cartRepository;
    public UpdateCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<Result> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cartRepository.UpdateCartItemQuantity(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);

        }
    }
}
