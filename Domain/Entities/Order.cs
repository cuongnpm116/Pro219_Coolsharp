using Domain.Enums;

namespace Domain.Entities;
public class Order
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public Guid? ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }

    public Guid? CustomerId { get; set; }
    public Guid? StaffId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Note { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipAddressDetail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }

    public virtual Staff? Staff { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
}
