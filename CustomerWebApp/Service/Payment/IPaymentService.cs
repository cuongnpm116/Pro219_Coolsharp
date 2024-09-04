using CustomerWebApp.Service.Payment.Dtos;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Payment;

public interface IPaymentService
{
    Task<Result<bool>> CreatePayment(CreatePaymentRequest request);
    string CreatePaymentUrl(HttpContext context, VnPayRequest model);
    PaymentResponse PaymentExecute(IQueryCollection collections);
}
