namespace CustomerWebApp.Service.Customer.Request;

public class UpdateProfileRequest
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
}
