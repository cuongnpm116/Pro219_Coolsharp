using System.Net.Http.Json;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace WebAppIntegrated.AddressService;
public sealed class VietNamAddressService : IVietNamAddressService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string StartUrl = "/api/vietnamaddress/";

    public VietNamAddressService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ProvinceVm>> GetProvinceListAsync()
    {
        HttpClient client = _httpClientFactory.CreateClient(ShopConstants.EShopClient);
        var result = await client.GetFromJsonAsync<IEnumerable<ProvinceVm>>(StartUrl + "province");
        return result;
    }

    public async Task<IEnumerable<DistrictVm>> GetDistrictListAsync(string provinceCode)
    {
        HttpClient client = _httpClientFactory.CreateClient(ShopConstants.EShopClient);
        string url = StartUrl + $"district?provincecode={provinceCode}";
        var result = await client.GetFromJsonAsync<IEnumerable<DistrictVm>>(url);
        return result;
    }

    public async Task<IEnumerable<WardVm>> GetWardListAsync(string districtCode)
    {
        HttpClient client = _httpClientFactory.CreateClient(ShopConstants.EShopClient);
        string url = StartUrl + $"ward?districtcode={districtCode}";
        var result = await client.GetFromJsonAsync<IEnumerable<WardVm>>(url);
        return result;
    }
}
