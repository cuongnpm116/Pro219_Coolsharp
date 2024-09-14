using CustomerWebApp.Service.Product.Dtos;
using CustomerWebApp.Service.Product.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Product;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<Dictionary<Guid, List<string>>>> GetDetailImage(Guid productId)
    {
        string url = $"/api/products/detail-image?productId={productId}";
        var result = await _httpClient.GetFromJsonAsync<Result<Dictionary<Guid, List<string>>>>(url);
        return result;
    }

    public async Task<Result<List<ProductVm>>> GetFeaturedProducts()
    {
        string url = $"/api/products/featured-product";
        var result = await _httpClient.GetFromJsonAsync<Result<List<ProductVm>>>(url);
        return result;
    }

    public async Task<Result<Guid>> GetProductDetailId(Guid productId, Guid colorId, Guid sizeId)
    {
        string url = $"/api/products/get-productdetailId?productid={productId}&colorid={colorId}&sizeid={sizeId}";
        var result = await _httpClient.GetFromJsonAsync<Result<Guid>>(url);
        return result;
    }

    public async Task<Result<ProductPriceVm>> GetProductDetailPrice(Guid productId, Guid colorId, Guid sizeId)
    {
        string url = $"/api/products/get-productdetail-price?productid={productId}&colorid={colorId}&sizeid={sizeId}";
        var result = await _httpClient.GetFromJsonAsync<Result<ProductPriceVm>>(url);
        return result;
    }

    public async Task<Result<int>> GetProductDetailStock(Guid productId, Guid colorId, Guid sizeId)
    {
        string url = $"/api/products/get-productdetail-stock?productid={productId}&colorid={colorId}&sizeid={sizeId}";
        var result = await _httpClient.GetFromJsonAsync<Result<int>>(url);
        return result;
    }

    public async Task<Result<ProductDetailVm>> ShowProductDetail(Guid productId)
    {
        var url = $"/api/Products/show-productDetail/{productId}";
        var result = await _httpClient.GetFromJsonAsync<Result<ProductDetailVm>>(url);
        return result;
    }

    public async Task<Result<PaginationResponse<ProductVm>>> ShowProductOnCustomerAppVm(ProductPaginationRequest request)
    {
        var url = $"/api/Products/get-product-paging?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (request.CategoryIds != null && request.CategoryIds.Count != 0)
        {
            var categoryIds = string.Join("&CategoryIds=", request.CategoryIds);
            url += $"&CategoryIds={categoryIds}";
        }

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }
        if (request.MinPrice.HasValue)
        {
            url += $"&MinPrice={request.MinPrice.Value}";
        }

        if (request.MaxPrice.HasValue)
        {
            url += $"&MaxPrice={request.MaxPrice.Value}";
        }

        var result = await _httpClient.GetFromJsonAsync<Result<PaginationResponse<ProductVm>>>(url);

        return result;
    }
}
