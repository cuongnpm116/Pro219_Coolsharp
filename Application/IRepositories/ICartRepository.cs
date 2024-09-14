using Application.Cqrs.Cart;
using Application.Cqrs.Cart.AddCart;
using Application.Cqrs.Cart.UpdateCart;
using Domain.Primitives;

namespace Application.IRepositories;
public interface ICartRepository
{
    Task<Result<CartVm>> GetCart(Guid customerId);
    Task<Result<bool>> AddToCart(AddCartCommand request);
    Task<Result<bool>> UpdateCartItemQuantity(UpdateCartCommand request);
    Task<Result<bool>> DeleteCartItem(List<Guid> productDetailIds);
}
