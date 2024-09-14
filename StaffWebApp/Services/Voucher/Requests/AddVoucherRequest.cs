namespace StaffWebApp.Services.Voucher.Dtos;

public class AddVoucherRequest
{
    public decimal DiscountCondition { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public DateTime StartedDate { get; set; } = DateTime.Now;
    public DateTime FinishedDate { get; set; } = DateTime.Now.AddDays(1);
    public int? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int Status { get; set; }
}
