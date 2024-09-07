using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Payment.CreatePayment;

public class CreatePaymentCommand : IRequest<Result>
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string StatusCode { get; set; } = "";
    public int PaymentStatus { get; set; }
    public int PaymentMethod { get; set; }

}
