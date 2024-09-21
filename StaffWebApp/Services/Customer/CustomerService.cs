using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Customer;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "/api/customers";

    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<PaginationResponse<CustomerVm>>> GetCustomersWithPagination(
        GetCustomerWithPaginationRequest request)
    {
        var finalUrl = _baseUrl + $"/list-customer?pagenumber={request.PageNumber}&pagesize={request.PageSize}";
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            finalUrl += $"&searchstring={request.SearchString}";
        }
        var apiRes = await _client.GetFromJsonAsync<Result<PaginationResponse<CustomerVm>>>(finalUrl);
        return apiRes;
    }
}
