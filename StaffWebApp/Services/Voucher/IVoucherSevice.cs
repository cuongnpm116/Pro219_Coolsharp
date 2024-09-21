using StaffWebApp.Services.Voucher.Requests;
using StaffWebApp.Services.Voucher.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Voucher;

public interface IVoucherSevice
{
    Task<Result<bool>> AddVoucher(AddVoucherRequest request);
    Task<Result<PaginationResponse<VoucherVm>>> GetVoucherPaging(GetVoucherPaginationRequest request);
    Task<Result<List<VoucherVm>>> GetListVoucher();
    Task<Result<VoucherDetailVm>> GetVoucherById(Guid Id);
    Task<Result<bool>> UpdateVoucher(UpdateVoucherRequest request);
}
