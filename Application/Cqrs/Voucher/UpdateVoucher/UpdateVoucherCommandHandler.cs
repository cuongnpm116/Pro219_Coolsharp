using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.UpdateVoucher;

internal sealed class UpdateVoucherCommandHandler : IRequestHandler<UpdateVoucherCommand, Result>
{
    private readonly IVoucherRepository _voucherRepository;
    public UpdateVoucherCommandHandler(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }
    public async Task<Result> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voucherRepository.UpdateVoucher(request);
            return result;
        }
        catch (Exception ex)
        {
             return Result<bool>.Error(ex.Message);
        }
    }
}
