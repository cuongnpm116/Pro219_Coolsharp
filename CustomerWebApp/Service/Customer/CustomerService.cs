using CustomerWebApp.Service.Customer.Request;
using CustomerWebApp.Service.Customer.Vms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Customer;

public class CustomerService : ICustomerService
{
    private const string startUrl = "/api/customers/";
    private readonly HttpClient _client;

    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }

    public async Task<Result<CustomerInfoVm>> GetUserProfile(Guid customerId)
    {
        string query = startUrl + $"get-customer-by-id?customerId={customerId}";
        var apiResponse = await _client.GetFromJsonAsync<Result<CustomerInfoVm>>(query);
        return apiResponse;
    }

    public async Task<Result<bool>> UpdateProfile(UpdateProfileRequest request)
    {
        string url = startUrl + "update-profile";
        var apiResponse = await _client.PutAsJsonAsync(url, request);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<bool>> UpdateAvatar(UpdateAvatarRequest request)
    {
        MultipartFormDataContent content = [];
        StreamContent fileContent = new(request.NewImage.OpenReadStream());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.NewImage.ContentType);
        content.Add(fileContent, "newImage", request.NewImage.Name);
        content.Add(new StringContent(request.OldImageUrl), "oldImageUrl");
        content.Add(new StringContent(request.CustomerId.ToString()), "customerId");

        string url = startUrl + "update-avatar";
        HttpResponseMessage apiResponse = await _client.PostAsync(url, content);
        string responseContent = await apiResponse.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(responseContent);
        return result;
    }

    public async Task<Result<bool>> Register(RegisterModel model)
    {
        var apiResponse = await _client.PostAsJsonAsync(startUrl + "create-customer", model);
        string content = await apiResponse.Content.ReadAsStringAsync();
        Result<bool>? result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }

    public async Task<Result<string>> Login(LoginInfo info)
    {
        var apiResponse = await _client.PostAsJsonAsync(startUrl + "login-customer", info);
        string content = await apiResponse.Content.ReadAsStringAsync();
        Result<string>? result = JsonConvert.DeserializeObject<Result<string>>(content);
        return result;
    }

    public async Task<bool> CheckUniqueEmail(string email)
    {
        string query = startUrl + $"check-unique-email?email={email}";
        Result<bool>? apiResponse = await _client.GetFromJsonAsync<Result<bool>>(query);
        return apiResponse.Value;
    }

    public async Task<bool> CheckUniqueUsername(string username)
    {
        string query = startUrl + $"check-unique-username?username={username}";
        Result<bool>? apiResponse = await _client.GetFromJsonAsync<Result<bool>>(query);
        return apiResponse.Value;
    }

    public async Task<Result<string>> ForgetPassword(string userInput)
    {
        string url = startUrl + $"forget-password?userinput={userInput}";
        var apiRes = await _client.GetFromJsonAsync<Result<string>>(url);
        return apiRes;
    }

    public async Task<Result<bool>> ChangePassword(ChangePasswordModel model)
    {
        string url = startUrl + "change-password";
        var apiRes = await _client.PutAsJsonAsync(url, model);
        string content = await apiRes.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }



}
