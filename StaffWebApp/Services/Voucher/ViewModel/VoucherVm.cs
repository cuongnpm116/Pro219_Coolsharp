namespace StaffWebApp.Services.Voucher.ViewModel;

public class VoucherVm
{
    public Guid Id { get; set; }
    public decimal DiscountCondition { get; set; }
    public string Name { get; set; } = string.Empty;
    public string VoucherCode { get; set; } = string.Empty;
    public int Stock { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime FinishedDate { get; set; }
    public int? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int Status { get; set; }
}
