using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Order;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private const string StartUrl = "/api/orders/";
    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<bool>> CancelOrder(CancelOrderRequest request)
    {
        string url = StartUrl + $"cancel-order";
        var apiRes = await _httpClient.PutAsJsonAsync(url, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
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

    public async Task<Result<IReadOnlyList<OrderDetailVm>>> GetOrderDetail(Guid orderId)
    {
        string url = StartUrl + $"get-orderdetails-for-customer?orderid={orderId}";
        var apiRes = await _httpClient.GetFromJsonAsync<Result<IReadOnlyList<OrderDetailVm>>>(url);
        return apiRes;
    }

    public async Task<Result<PaginationResponse<OrderVm>>> GetOrdersForCustomerWithPagination(GetOrdersWithPaginationRequest request)
    {
        
        StringBuilder sb = new(StartUrl);

        sb.Append("get-orders-for-customer?");
        sb.Append($"customerid={request.CustomerId}&");
        sb.Append($"pagesize={request.PageSize}&");
        sb.Append($"pagenumber={request.PageNumber}&");
        sb.Append($"searchstring={request.SearchString}&"); 
        sb.Append($"orderstatus={request.OrderStatus}");

        var result = await _httpClient.GetFromJsonAsync<Result<PaginationResponse<OrderVm>>>(sb.ToString());
        return result;
    }
}
