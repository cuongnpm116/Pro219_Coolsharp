using Newtonsoft.Json;
using StaffWebApp.Services.Category.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace StaffWebApp.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private const string _baseUrl = "/api/categories";

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<List<CategoryVm>>> Categories()
    {
        var result = await _httpClient.GetFromJsonAsync<Result<List<CategoryVm>>>("/api/categories");
        return result;
    }

    public async Task<bool> CheckCategoryNameExist(string name)
    {
        string url = _baseUrl + $"/check-exist?name={name}";
        var result = await _httpClient.GetFromJsonAsync<Result<bool>>(url);
        return result.Value;
    }

    public async Task<bool> CreateCategory(string name)
    {
        string url = _baseUrl + "/create";
        var apiRes = await _httpClient.PostAsJsonAsync(url, name);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result.Value;
    }
}
