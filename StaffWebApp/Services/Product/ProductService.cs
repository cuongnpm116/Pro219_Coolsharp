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
    public async Task<Result<PaginationResponse<ProductVm>>> ShowProduct(ProductPaginationRequest request)
    {
        var url = _baseUrl + $"/get-product-staff-paging?PageNumber={request.PageNumber}&PageSize={request.PageSize}";
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            url += $"&CategoryName={Uri.EscapeDataString(request.CategoryName)}";
        }

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await _client.GetFromJsonAsync<Result<PaginationResponse<ProductVm>>>(url);
        return result;
    }

    public async Task<bool> CreateProductAsync(
        ProductInfoVm info,
        List<ProductDetailVm> details,
        Dictionary<Guid, List<ImageVm>> imagesByColor)
    {
        // 1. tạo createImageRequest
        Dictionary<Guid, List<CreateImageRequest>> imagesNameByColor = [];
        foreach (var key in imagesByColor.Keys)
        {
            imagesNameByColor.Add(key, []);
            foreach (ImageVm image in imagesByColor[key])
            {
                string newFileName = await _grpcService.UploadToServer(image.File, DirectoryConstants.ProductContent);
                imagesNameByColor[key].Add(new(Guid.NewGuid(), newFileName));
            }
        }

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
            {
                CreateProductImageRequest productImageRequest = new(detail.Id, item.Id);
                productImageRequests.Add(productImageRequest);
            }
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
    }
}
