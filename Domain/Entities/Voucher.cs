using Domain.Enums;

namespace Domain.Entities;

public class Voucher
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime ModifiedOn { get; set; }
    public decimal DiscountCondition { get; set; }
    public string Name { get; set; } = string.Empty;
    public string VoucherCode { get; set; } = string.Empty;
    public int Stock { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime FinishedDate { get; set; }
    public int? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<Order>? Orders { get; set; }
}
