namespace StaffWebApp.Services.Order.Requests;

public class CancelOrderRequest
{
    public Guid OrderId { get; set; }
    public Guid? ModifiedBy { get; set; }
}
