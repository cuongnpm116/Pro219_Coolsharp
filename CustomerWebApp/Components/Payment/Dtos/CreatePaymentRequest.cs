namespace CustomerWebApp.Components.Payment.Dtos;

public class CreatePaymentRequest
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string StatusCode { get; set; } = "";
    public int PaymentStatus { get; set; }
    public int PaymentMethod { get; set; }
}
