namespace CustomerWebApp.Components.Orders.ViewModel;

public class OrderWithPaymentVm
{
    public Guid OrderId { get; set; }
    public string OrderCode { get; set; }
    public Guid? CustomerId { get; set; }
}
