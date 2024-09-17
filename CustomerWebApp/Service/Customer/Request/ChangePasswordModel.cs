namespace CustomerWebApp.Service.Customer.Request;

public class ChangePasswordModel
{
    public string Username { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
