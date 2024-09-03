using StaffWebApp.Services.Size.Requests;
using StaffWebApp.Services.Size.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Size
{
    public class SizeService : ISizeService
    {
        private const string apiUrl = "/api/Sizes";
        private readonly IHttpClientFactory _httpClientFactory;
        public SizeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<Result<bool>> CreateSize(CreateSizeRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("eShopApi");
            var response = await httpClient.PostAsJsonAsync(apiUrl, request);
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
            var httpClient = _httpClientFactory.CreateClient("eShopApi");
            var response = await httpClient.GetFromJsonAsync<Result<SizeVm>>(apiUrl + $"/GetSizeById?Id={id}");
            return response;
        }

        public async Task<Result<PaginationResponse<SizeVm>>> GetSizesWithPagination(SizePaginationRequest request)
        {

            var httpClient = _httpClientFactory.CreateClient("eShopApi");
            var url = apiUrl + $"?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
            }

            var result = await httpClient.GetFromJsonAsync<Result<PaginationResponse<SizeVm>>>(url);
            return result;
        }



        public async Task<Result<bool>> UpdateSize(UpdateSizeRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("eShopApi");
            var response = await httpClient.PutAsJsonAsync(apiUrl, request);
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
    }
}
