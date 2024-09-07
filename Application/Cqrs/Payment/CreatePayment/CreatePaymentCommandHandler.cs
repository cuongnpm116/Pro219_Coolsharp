using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Payment.CreatePayment;

internal sealed class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
{
    private IPaymentRepository _paymentRepository;
    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _paymentRepository.CreatePayment(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
