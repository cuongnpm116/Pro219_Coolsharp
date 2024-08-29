using StaffWebApp.Components.Staff.Vms;
using StaffWebApp.Services.Staff.Requests;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Staff;

public class StaffService : IStaffService
{
    private readonly HttpClient _client;
    private string _baseUrl = "api/staff";

    public StaffService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<PaginationResponse<StaffVm>>> GetListStaffWithPagination(GetListStaffRequest request)
    {
        string url = _baseUrl + $"/list-staff-with-pagination?PageNumber={request.PageNumber}&PageSize={request.PageSize}&SearchString={request.SearchString}&Status={request.Status}";
        var apiRes = await _client.GetFromJsonAsync<Result<PaginationResponse<StaffVm>>>(url);
        return apiRes;
    }
}
