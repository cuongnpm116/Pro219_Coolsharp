using CustomerWebApp.Service.Address.Dtos;
using CustomerWebApp.Service.Address.ViewModel;
using Newtonsoft.Json;
using WebAppIntegrated.AddressService;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Address;

public class AddressService : IAddressService
{
    private readonly HttpClient _httpClient;
    private const string startUrl = "/api/address/";
    public AddressService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<List<AddressModel>>> GetUserAddress(Guid customerId)
    {

        string url = startUrl + $"get-addresses?customerId={customerId}";
        var result = await _httpClient.GetFromJsonAsync<Result<List<AddressModel>>>(url);
        return result;
    }

    public async Task<Result<bool>> AddUserAddress(AddAddressRequest model)
    {

        string url = startUrl + "add-address";
        HttpResponseMessage apiResponse = await _httpClient.PostAsJsonAsync(url, model);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<bool>> UpdateUserAddress(UpdateAddressRequest model)
    {

        string url = startUrl + "update-address";
        HttpResponseMessage apiResponse = await _httpClient.PutAsJsonAsync(url, model);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<AddressModel>> GetAddressById(Guid addressId)
    {

        string url = startUrl + $"get-address-by-id?addressId={addressId}";
        Result<AddressModel> result = await _httpClient.GetFromJsonAsync<Result<AddressModel>>(url);
        return result;
    }

    public async Task<Result<bool>> MakeDefaultAddress(MakeDefaultAddressModel model)
    {

        string url = startUrl + "make-default-address";
        HttpResponseMessage apiResponse = await _httpClient.PutAsJsonAsync(url, model);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<bool>> DeleteAddress(DeleteAddressRequest model)
    {

        string url = startUrl + "delete-address";
        HttpResponseMessage apiResponse = await _httpClient.PutAsJsonAsync(url, model);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<AddressModel>> GetDefaultAddress(Guid customerId)
    {

        string url = startUrl + $"default-address?customerId={customerId}";
        var apiRes = await _httpClient.GetFromJsonAsync<Result<AddressModel>>(url);
        return apiRes;
    }
}
