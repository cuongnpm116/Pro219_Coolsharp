using Newtonsoft.Json;
using StaffWebApp.Services.Staff.Requests;
using StaffWebApp.Services.Staff.Vms;
using System.Net.Http;
using System.Text;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Staff;
internal class StaffService : IStaffService
{
    private readonly HttpClient _client;
    private string _baseUrl = "api/staff";

    public StaffService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<bool>> AddStaff(AddStaffRequest request)
    {
        string url = _baseUrl + "/add-staff";
        var apiRes = await _client.PostAsJsonAsync(url, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }

    public async Task<Result<PaginationResponse<StaffVm>>> GetListStaffWithPagination(GetListStaffRequest request)
    {
        StringBuilder sb = new();
        sb.Append(_baseUrl)
            .Append("/list-staff-with-pagination?")
            .AppendFormat("PageNumber={0}&PageSize={1}&Status={2}&", request.PageNumber, request.PageSize, request.Status);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            sb.AppendFormat("SearchString={0}&", request.SearchString);
        }

        string url = sb.ToString().TrimEnd('&');

        var apiRes = await _client.GetFromJsonAsync<Result<PaginationResponse<StaffVm>>>(url);
        return apiRes;
    }

    public async Task<Result<UpdateStaffInfoVm>> GetStaffInfoToUpdate(Guid staffId)
    {
        string url = _baseUrl + $"/get-staff-update-info?staffid={staffId}";
        var apiRes = await _client.GetFromJsonAsync<Result<UpdateStaffInfoVm>>(url);
        return apiRes;
    }

    public async Task<Result<string>> Login(LoginInfo info)
    {
        HttpResponseMessage apiResponse = await _client.PostAsJsonAsync(_baseUrl + "/login", info);
        string content = await apiResponse.Content.ReadAsStringAsync();
        Result<string>? result = JsonConvert.DeserializeObject<Result<string>>(content);
        return result;
    }

    public async Task<Result<bool>> UpdateStaffInfo(UpdateStaffInfoVm request)
    {
        string url = _baseUrl + "/update-staff-info";
        var apiRes = await _client.PutAsJsonAsync(url, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }

    public async Task<Result<bool>> UpdateStaffRole(UpdateStaffRoleRequest request)
    {
        string url = _baseUrl + "/update-staff-role";
        var apiRes = await _client.PutAsJsonAsync(url, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }
}
