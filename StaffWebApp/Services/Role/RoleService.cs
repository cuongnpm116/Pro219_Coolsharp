using Newtonsoft.Json;
using StaffWebApp.Services.Role.Requests;
using StaffWebApp.Services.Role.Vms;
using System.Text;
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

    public async Task<bool> CheckUniqueRoleCode(string code)
    {
        // trick để không bị 400 khi code == string.Empty
        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }
        string finalUrl = _baseUrl + $"/check-unique-role-code?code={code}";
        bool result = await _client.GetFromJsonAsync<bool>(finalUrl);
        return result;
    }

    public async Task<bool> CheckUniqueRoleName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        string finalUrl = _baseUrl + $"/check-unique-role-name?name={name}";
        bool result = await _client.GetFromJsonAsync<bool>(finalUrl);
        return result;
    }

    public async Task<bool> CreateRole(CreateRoleRequest request)
    {
        string finalUrl = _baseUrl + "/create-role";
        var apiRes = await _client.PostAsJsonAsync(finalUrl, request);
        return await apiRes.Content.ReadFromJsonAsync<bool>();
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
