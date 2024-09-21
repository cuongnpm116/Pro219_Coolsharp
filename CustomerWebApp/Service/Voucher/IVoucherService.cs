using CustomerWebApp.Service.Voucher.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Voucher;

public interface IVoucherService
{
    Task<Result<List<VoucherVm>>> GetListVoucher();
}
