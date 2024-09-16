namespace CustomerWebApp.Service.Order.Dtos;

public class CancelOrderRequest
{
    public Guid OrderId { get; set; }
    public Guid? ModifiedBy { get; set; }
}
