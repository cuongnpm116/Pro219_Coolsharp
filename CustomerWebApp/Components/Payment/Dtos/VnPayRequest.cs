namespace CustomerWebApp.Components.Payment.Dtos;

public class VnPayRequest
{
    public string OrderCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public DateTime CreatedDate { get; set; }
}
