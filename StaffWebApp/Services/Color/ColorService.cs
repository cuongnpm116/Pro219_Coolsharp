using StaffWebApp.Services.Color.Requests;
using StaffWebApp.Services.Color.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Color;

public class ColorService : IColorService
{
    private const string _baseUrl = "/api/Colors";
    private readonly HttpClient _client;

    public ColorService(IHttpClientFactory _httpClientFactory)
    {
        _client = _httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<bool>> CreateColor(CreateColorRequest request)
    {
        var response = await _client.PostAsJsonAsync(_baseUrl, request);
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
        var response = await _client.GetFromJsonAsync<Result<ColorVm>>(_baseUrl + $"/GetColorById?Id={id}");
        return response;
    }

    public async Task<Result<PaginationResponse<ColorVm>>> GetColorsWithPagination(ColorPaginationRequest request)
    {

        var url = _baseUrl + $"?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await _client.GetFromJsonAsync<Result<PaginationResponse<ColorVm>>>(url);
        return result;
    }



    public async Task<Result<bool>> UpdateColor(UpdateColorRequest request)
    {
        var response = await _client.PutAsJsonAsync(_baseUrl, request);

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

    public async Task<IEnumerable<ColorForSelectVm>> GetColorForSelectVms()
    {
        string finalUrl = _baseUrl + "/color-for-select";
        var apiRes = await _client.GetAsync(finalUrl);
        if (apiRes.IsSuccessStatusCode)
        {
            var result = await apiRes.Content.ReadFromJsonAsync<IEnumerable<ColorForSelectVm>>() ?? [];
            return result;
        }
        return [];
    }
}
