using CustomerWebApp.Service.Cart.Dtos;
using CustomerWebApp.Service.Cart.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Cart;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    public CartService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<bool>> AddToCart(AddCartRequest request)
    {

        var response = await _httpClient.PostAsJsonAsync("/api/carts/add-to-cart", request);

        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            result = responseBody;
        }
        return result;
    }

    public async Task<Result<bool>> DeleteCartItem(DeleteCartRequest request)
    {

        var url = $"/api/carts/delete-cart-item";
        if (request.ProductDetailIds != null && request.ProductDetailIds.Count != 0)
        {
            var cartItemIds = string.Join("&ProductDetailIds=", request.ProductDetailIds);
            url += $"?ProductDetailIds={cartItemIds}";
        }
        var result = new Result<bool>();
        var response = await _httpClient.DeleteAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            result = responseBody;
        }

        return result;
    }

    public async Task<Result<CartVm>> GetCart(Guid userId)
    {
        var response = await _httpClient.GetFromJsonAsync<Result<CartVm>>($"/api/carts?CustomerId={userId}");

        return response;
    }

    public async Task<Result<bool>> UpdateCartItemQuantity(UpdateCartRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/carts/update-quantity", request);

        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            result = responseBody;
        }

        return result;
    }
}
