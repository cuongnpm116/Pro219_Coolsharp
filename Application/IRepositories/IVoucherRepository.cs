using Application.Cqrs.Voucher.AddVoucher;
using Application.Cqrs.Voucher.GetVoucherPaging;
using Application.Cqrs.Voucher;
using Domain.Primitives;
using Application.ValueObjects.Pagination;
using Application.Cqrs.Voucher.UpdateVoucher;

namespace Application.IRepositories;

public interface IVoucherRepository
{
    Task<Result<bool>> AddVoucher(AddVoucherCommand command);
    Task<Result<bool>> UpdateVoucher(UpdateVoucherCommand command);
    Task<Result<PaginationResponse<VoucherVm>>> GetVoucherPaging(GetVoucherPaginationQuery query);
    Task<Result<List<VoucherVm>>> GetListVoucher();
    Task<Result<VoucherDetailVm>> GetVoucherById(Guid Id);
}
