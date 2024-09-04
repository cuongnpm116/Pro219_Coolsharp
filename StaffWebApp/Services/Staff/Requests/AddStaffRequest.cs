using StaffWebApp.Services.Role.Vms;

namespace StaffWebApp.Services.Staff.Requests;
public class AddStaffRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<Guid> RoleIds { get; set; } = [];

    public AddStaffRequest(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string email,
        string username,
        string password,
        IEnumerable<RoleVmForGetAll> roles)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Username = username;
        Password = password;
        RoleIds = roles.Select(x => x.Id).ToList();
    }
}


