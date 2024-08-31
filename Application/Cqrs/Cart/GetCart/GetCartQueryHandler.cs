using Application.IRepositories;
using Domain.Primitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Cart.GetCart;

internal sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result>
{
    private readonly ICartRepository _cartRepository;
    public GetCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cartRepository.GetCart(request.CustomerId);
            return result;
        }
        catch (Exception ex)
        {
            return Result<CartVm>.Error(ex.Message);

        }
    }
}
