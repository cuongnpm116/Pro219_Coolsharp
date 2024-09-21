namespace StaffWebApp.Services.Customer;

public class CustomerVm
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string ImageUrl { get; set; }
    public DateTime Dob { get; set; }
    public string EmailAddress { get; set; }
    public string Username { get; set; }
    public int Gender { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}
