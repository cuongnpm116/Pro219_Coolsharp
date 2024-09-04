using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.AddVoucher;

internal sealed class AddVoucherCommandHandler : IRequestHandler<AddVoucherCommand, Result>
{
    private readonly IVoucherRepository _voucherRepository;
    public AddVoucherCommandHandler(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }

    public async Task<Result> Handle(AddVoucherCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voucherRepository.AddVoucher(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
