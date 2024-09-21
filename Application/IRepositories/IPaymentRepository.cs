using Application.Cqrs.Payment.CreatePayment;
using Domain.Primitives;

namespace Application.IRepositories;

public interface IPaymentRepository
{
    Task<Result<bool>> CreatePayment(CreatePaymentCommand command);
}

