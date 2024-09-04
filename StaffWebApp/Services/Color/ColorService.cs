using StaffWebApp.Services.Color.Requests;
using StaffWebApp.Services.Color.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Color;

public class ColorService : IColorService
{
    private const string apiUrl = "/api/Colors";
    private readonly IHttpClientFactory _httpClientFactory;

    public ColorService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<bool>> CreateColor(CreateColorRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.PostAsJsonAsync(apiUrl, request);
        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            if (responseBody != null)
            {
                result.IsSuccess = responseBody.IsSuccess;
                result.Message = responseBody.Message;
                result.Value = responseBody.Value;
                result.Status = responseBody.Status;
            }

        }
        else
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            if (responseBody != null)
            {
                result.IsSuccess = responseBody.IsSuccess;
                result.Message = responseBody.Message;
                result.Value = responseBody.Value;
                result.Status = responseBody.Status;
            }
        }

        return result;

    }

    public async Task<Result<ColorVm>> GetColorById(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.GetFromJsonAsync<Result<ColorVm>>(apiUrl + $"/GetColorById?Id={id}");
        return response;
    }

    public async Task<Result<PaginationResponse<ColorVm>>> GetColorsWithPagination(ColorPaginationRequest request)
    {

        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var url = apiUrl + $"?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await httpClient.GetFromJsonAsync<Result<PaginationResponse<ColorVm>>>(url);
        return result;
    }



    public async Task<Result<bool>> UpdateColor(UpdateColorRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.PutAsJsonAsync(apiUrl, request);

        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            if (responseBody != null)
            {
                result.IsSuccess = responseBody.IsSuccess;
                result.Message = responseBody.Message;
                result.Value = responseBody.Value;
                result.Status = responseBody.Status;
            }

        }
        else
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            if (responseBody != null)
            {
                result.IsSuccess = responseBody.IsSuccess;
                result.Message = responseBody.Message;
                result.Value = responseBody.Value;
                result.Status = responseBody.Status;
            }
        }

        return result;
    }
}
