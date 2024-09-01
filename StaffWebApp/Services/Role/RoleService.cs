using StaffWebApp.Services.Role.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

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
        string url = _baseUrl + "/get-roleids-by-staff-id";
        var apiRes = _client.GetFromJsonAsync<Result<List<Guid>>>(url);
        return apiRes;
    }

    public Task<Result<List<RoleVm>>> GetRoles()
    {
        string url = _baseUrl + "/get-roles";
        var apiRes = _client.GetFromJsonAsync<Result<List<RoleVm>>>(url);
        return apiRes;
    }
}
