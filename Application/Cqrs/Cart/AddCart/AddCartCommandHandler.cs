

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Cart.AddCart;

internal sealed class AddCartCommandHandler : IRequestHandler<AddCartCommand, Result>
{
    private readonly ICartRepository _cartRepository;
    public AddCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<Result> Handle(AddCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cartRepository.AddToCart(request);

            return result;
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);

        }
    }
}
