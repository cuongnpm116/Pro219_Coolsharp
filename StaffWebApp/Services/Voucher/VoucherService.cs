using StaffWebApp.Services.Voucher.Requests;
using StaffWebApp.Services.Voucher.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Voucher;

public class VoucherService : IVoucherSevice
{
    private readonly HttpClient _httpClient;

    public VoucherService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<bool>> AddVoucher(AddVoucherRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Vouchers/create-voucher", request);

        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            result = responseBody;

        }

        return result;
    }

    public async Task<Result<List<VoucherVm>>> GetListVoucher()
    {
        var response = await _httpClient.GetFromJsonAsync<Result<List<VoucherVm>>>("/api/Vouchers/get-list-voucher");
        return response;
    }

    public async Task<Result<VoucherDetailVm>> GetVoucherById(Guid Id)
    {
        var response = await _httpClient.GetFromJsonAsync<Result<VoucherDetailVm>>($"/api/Vouchers/get-voucher/{Id}");
        return response;
    }

    public async Task<Result<PaginationResponse<VoucherVm>>> GetVoucherPaging(GetVoucherPaginationRequest request)
    {
        var url = $"/api/Vouchers/get-voucher-paging?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await _httpClient.GetFromJsonAsync<Result<PaginationResponse<VoucherVm>>>(url);
        return result;
    }

    public async Task<Result<bool>> UpdateVoucher(UpdateVoucherRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/Vouchers/update-voucher", request);

        var result = new Result<bool>();

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadFromJsonAsync<Result<bool>>();
            result = responseBody;

        }

        return result;
    }
}
