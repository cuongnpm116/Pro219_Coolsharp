using CustomerWebApp.Components.Voucher.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Service.Voucher;

public class VoucherService : IVoucherService
{
    private readonly HttpClient _httpClient;

    public VoucherService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
    }
    public async Task<Result<List<VoucherVm>>> GetListVoucher()
    {

        var response = await _httpClient.GetFromJsonAsync<Result<List<VoucherVm>>>("/api/Vouchers/get-list-voucher");
        return response;
    }
}
