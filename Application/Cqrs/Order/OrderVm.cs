using Domain.Enums;

namespace Application.Cqrs.Order;

public class OrderVm
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public string VoucherCode { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Note { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipAddressDetail { get; set; } = string.Empty;
    public string Customer { get; set; }
    public string? Staff { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal? TotalPrice { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public List<OrderDetailVm> Details { get; set; }
}
