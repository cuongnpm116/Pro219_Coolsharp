using Newtonsoft.Json;
using StaffWebApp.Services.Product.Dtos;
using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Requests.Create;
using StaffWebApp.Services.Product.Requests.Update;
using StaffWebApp.Services.Product.Vms;
using StaffWebApp.Services.Product.Vms.Create;
using System.Text;
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
    {
        // 1. tạo createImageRequest
        Dictionary<Guid, List<ImageDto>> imagesNameByColor = [];
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
        List<ProductDetailDto> productDetailRequests = [];
        List<CreateProductImageRequest> productImageRequests = [];
        foreach (ProductDetailVm detail in details)
        {
            ProductDetailDto deatilRequest = new(
                detail.Id,
                detail.Color.Id,
                detail.Size.Id,
                detail.Stock,
                detail.Price,
                detail.OriginalPrice);
            productDetailRequests.Add(deatilRequest);
            List<ImageDto> imageByColor = imagesNameByColor[detail.Color.Id];
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

    public async Task<ProductInfoDto> GetProductInfo(Guid productId)
    {
        string finalUrl = _baseUrl + $"/get-product-info?productid={productId}";
        var apiRes = await _client.GetFromJsonAsync<ProductInfoDto>(finalUrl);
        return apiRes;
    }

    public async Task<PaginationResponse<ProductVm>> GetProducts(GetProductPaginationRequest request)
    {
        StringBuilder urlBuilder = new(_baseUrl);
        urlBuilder.Append("/get-product-for-staff");

        urlBuilder.Append($"?pageNumber={request.PageNumber}&pageSize={request.PageSize}");

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            urlBuilder.Append($"&searchString={Uri.EscapeDataString(request.SearchString)}");
        }

        if (!string.IsNullOrEmpty(request.CategoryId))
        {
            urlBuilder.Append($"&categoryId={Uri.EscapeDataString(request.CategoryId)}");
        }

        string finalUrl = urlBuilder.ToString();

        var apiRes = await _client.GetFromJsonAsync<PaginationResponse<ProductVm>>(finalUrl);
        return apiRes;
    }

    public async Task<bool> UpdateProductInfoAsync(Guid productId, ProductInfoVm info)
    {
        UpdateProductInfoRequest request = new(
            productId,
            info.Name,
            info.Categories.Select(x => x.CategoryId));

        string finalUrl = $"{_baseUrl}/update-info";
        var apiRes = await _client.PutAsJsonAsync(finalUrl, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        bool value = JsonConvert.DeserializeObject<bool>(content);
        return value;
    }

    public async Task<IEnumerable<DetailVm>> GetDetails(Guid productId)
    {
        string finalUrl = _baseUrl + $"/get-details-for-staff?productid={productId}";
        var apiRes = await _client.GetFromJsonAsync<IEnumerable<DetailVm>>(finalUrl);
        return apiRes;
    }

    public async Task<bool> UpdateDetailAsync(DetailVm detail)
    {
        UpdateProductDetailRequest request = new(
            detail.Id,
            detail.Price,
            detail.OriginalPrice,
            detail.Stock,
            detail.Color.Id,
            detail.Size.Id);

        string finalUrl = $"{_baseUrl}/update-detail";
        var apiRes = await _client.PutAsJsonAsync(finalUrl, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        bool value = JsonConvert.DeserializeObject<bool>(content);
        return value;
    }
}
