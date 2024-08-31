using CustomerWebApp.Components.Carts.Dtos;
using CustomerWebApp.Components.Carts.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Cart;

public interface ICartService
{
    Task<Result<CartVm>> GetCart(Guid userId);
    Task<Result<bool>> AddToCart(AddCartRequest request);
    Task<Result<bool>> UpdateCartItemQuantity(UpdateCartRequest request);
    Task<Result<bool>> DeleteCartItem(DeleteCartRequest request);
}
