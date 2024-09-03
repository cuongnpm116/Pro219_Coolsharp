using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.GetVoucherPaging;

internal sealed class GetVoucherPaginationQueryHandler : IRequestHandler<GetVoucherPaginationQuery, Result>
{
    private readonly IVoucherRepository _voucherRepository;
    public GetVoucherPaginationQueryHandler(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }
    public async Task<Result> Handle(GetVoucherPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voucherRepository.GetVoucherPaging(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
