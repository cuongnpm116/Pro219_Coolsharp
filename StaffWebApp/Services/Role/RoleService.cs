using System.Text;
using Newtonsoft.Json;
using StaffWebApp.Services.Role.Requests;
using StaffWebApp.Services.Role.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Role;
public class RoleService : IRoleService
{
    private readonly HttpClient _client;
    private string _baseUrl = "api/role";

    public RoleService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public Task<Result<List<Guid>>> GetRoleIdsByStaffId(Guid staffId)
    {
        string url = _baseUrl + $"/get-roleids-by-staff-id?staffid={staffId}";
        var apiRes = _client.GetFromJsonAsync<Result<List<Guid>>>(url);
        return apiRes;
    }

    public Task<Result<List<RoleVmForGetAll>>> GetRoles()
    {
        string url = _baseUrl + "/get-roles";
        var apiRes = _client.GetFromJsonAsync<Result<List<RoleVmForGetAll>>>(url);
        return apiRes;
    }

    public Task<Result<PaginationResponse<RoleVm>>> GetRolesWithPagination(GetRolesWithPaginationRequest request)
    {
        StringBuilder sb = new(_baseUrl);
        sb.Append("/get-roles-with-pagination?")
            .Append($"PageSize={request.PageSize}&")
            .Append($"PageNumber={request.PageNumber}&");
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            sb.Append($"SearchString={Uri.EscapeDataString(request.SearchString)}&");
        }
        string finalUrl = sb.ToString().TrimEnd('&');
        var apiRes = _client.GetFromJsonAsync<Result<PaginationResponse<RoleVm>>>(finalUrl);
        return apiRes;
    }

    public async Task<Result<bool>> UpdateRole(RoleVm role)
    {
        string url = _baseUrl + "/update-role";
        var apiRes = await _client.PutAsJsonAsync(url, role);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }
}
