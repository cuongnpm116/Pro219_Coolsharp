

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.GetVoucherById;

internal sealed class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, Result>
{
    private readonly IVoucherRepository _voucherRepository;
    public GetVoucherByIdQueryHandler(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }
    public async Task<Result> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voucherRepository.GetVoucherById(request.Id);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
