using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using System.Net.Http;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Order;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<OrderWithPaymentVm>> CreateOrder(CreateOrderRequest request)
    {
        
        var response = await _httpClient.PostAsJsonAsync("/api/orders/create-order", request);

        var result = new Result<OrderWithPaymentVm>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<OrderWithPaymentVm>>();
            result = responseBody;
        }

        return result;
    }
}
