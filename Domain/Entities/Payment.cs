using Domain.Enums;

namespace Domain.Entities;
public class Payment
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Order? Order { get; set; }
}
