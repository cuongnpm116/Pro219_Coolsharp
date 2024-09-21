namespace CustomerWebApp.Service.Customer.Vms;

public class CustomerInfoVm
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
