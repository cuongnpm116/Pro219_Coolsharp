using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product;

public class ProductService : IProductService
{
    private const string apiUrl = "/api/Products/";
    private readonly HttpClient _client;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<PaginationResponse<ProductVm>>> ShowProduct(ProductPaginationRequest request)
    {
        var url = apiUrl + "get-product-staff-paging?";
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            url += $"CategoryName={Uri.EscapeDataString(request.CategoryName)}";
        }
        url += $"&PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await _client.GetFromJsonAsync<Result<PaginationResponse<ProductVm>>>(url);
        return result;
    }
}
