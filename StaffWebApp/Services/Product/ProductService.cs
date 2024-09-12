using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms.Create;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Grpc;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product;

public class ProductService : IProductService
{
    private const string _baseUrl = "/api/products";
    private readonly HttpClient _client;
    private readonly IGrpcService _grpcService;

    public ProductService(IHttpClientFactory httpClientFactory, IGrpcService grpcService)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
        _grpcService = grpcService;
    }

    public async Task<bool> CreateProductAsync(
        ProductInfoVm info,
        List<ProductDetailVm> details,
        Dictionary<Guid, List<ImageVm>> imagesByColor)
    public async Task<Result<PaginationResponse<ProductVm>>> ShowProduct(ProductPaginationRequest request)
    {
        // 1. tạo createImageRequest
        Dictionary<Guid, List<CreateImageRequest>> imagesNameByColor = [];
        foreach (var key in imagesByColor.Keys)
        var url = apiUrl + "get-product-staff-paging?";
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            imagesNameByColor.Add(key, []);
            foreach (ImageVm image in imagesByColor[key])
            {
                string newFileName = await _grpcService.UploadToServer(image.File, DirectoryConstants.ProductContent);
                imagesNameByColor[key].Add(new(Guid.NewGuid(), newFileName));
            url += $"CategoryName={Uri.EscapeDataString(request.CategoryName)}";
            }
        }
        url += $"&PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        // 2. tạo trung gian theo màu
        List<CreateProductDetailRequest> productDetailRequests = [];
        List<CreateProductImageRequest> productImageRequests = [];
        foreach (ProductDetailVm detail in details)
        {
            CreateProductDetailRequest deatilRequest = new(
                detail.Id,
                detail.Color.Id,
                detail.Size.Id,
                detail.Stock,
                detail.Price,
                detail.OriginalPrice);
            productDetailRequests.Add(deatilRequest);
            List<CreateImageRequest> imageByColor = imagesNameByColor[detail.Color.Id];
            foreach (var item in imageByColor)
        if (!string.IsNullOrEmpty(request.SearchString))
            {
                CreateProductImageRequest productImageRequest = new(detail.Id, item.Id);
                productImageRequests.Add(productImageRequest);
            }
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        // 3. tạo productRequest
        CreateProductRequest request = new(
            info.Name,
            info.Categories.Select(x => x.CategoryId),
            imagesNameByColor.Values.SelectMany(x => x).ToList(),
            productDetailRequests,
            productImageRequests);

        string finalUrl = $"{_baseUrl}/create-product";
        var apiRes = await _client.PostAsJsonAsync(finalUrl, request);
        return apiRes.IsSuccessStatusCode;
        var result = await _client.GetFromJsonAsync<Result<PaginationResponse<ProductVm>>>(url);
        return result;
    }
}
