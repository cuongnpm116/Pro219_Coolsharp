using StaffWebApp.Services.Size.Requests;
using StaffWebApp.Services.Size.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Size;

public class SizeService : ISizeService
{
    private const string apiUrl = "/api/Sizes";
    private readonly HttpClient _client;

    public SizeService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }


    public async Task<Result<bool>> CreateSize(CreateSizeRequest request)
    {
        var response = await _client.PostAsJsonAsync(apiUrl, request);
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

    public async Task<Result<SizeVm>> GetSizeById(Guid id)
    {
        var response = await _client.GetFromJsonAsync<Result<SizeVm>>(apiUrl + $"/GetSizeById?Id={id}");
        return response;
    }

    public async Task<Result<PaginationResponse<SizeVm>>> GetSizesWithPagination(SizePaginationRequest request)
    {
        var url = apiUrl + $"?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await _client.GetFromJsonAsync<Result<PaginationResponse<SizeVm>>>(url);
        return result;
    }



    public async Task<Result<bool>> UpdateSize(UpdateSizeRequest request)
    {
        var response = await _client.PutAsJsonAsync(apiUrl, request);
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

    public async Task<IEnumerable<SizeForSelectVm>> GetSizeForSelectVms()
    {
        string finalUrl = apiUrl + "/size-for-select";
        var apiRes = await _client.GetAsync(finalUrl);
        if (apiRes.IsSuccessStatusCode)
        {
            var result = await apiRes.Content.ReadFromJsonAsync<IEnumerable<SizeForSelectVm>>() ?? [];
            return result;
        }
        return [];
    }
}
