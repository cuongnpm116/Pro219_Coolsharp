using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.GetListVoucher;

internal sealed class GetListVoucherQueryHandler : IRequestHandler<GetListVoucherQuery, Result>
{
    private readonly IVoucherRepository _voucherRepository;
    public GetListVoucherQueryHandler(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }
    public async Task<Result> Handle(GetListVoucherQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voucherRepository.GetListVoucher();
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
