using CustomerWebApp.Service.Category.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Category;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(IHttpClientFactory httpClientFactory)
    {

        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);

    }

    public async Task<Result<List<CategoryVm>>> Categories()
    {
        Result<List<CategoryVm>>? result = await _httpClient.GetFromJsonAsync<Result<List<CategoryVm>>>("/api/categories");
        return result;
    }
}
