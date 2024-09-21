using Domain.Enums;

namespace Application.Cqrs.Staff;
public class StaffVm
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public DateTime Dob { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Roles { get; private set; } = string.Empty;
    public Status Status { get; private set; }

    public StaffVm(Guid id, string fullName, DateTime dob, string email, string username, string roles, Status status)
    {
        Id = id;
        FullName = fullName;
        Dob = dob;
        Email = email;
        Username = username;
        Roles = roles;
        Status = status;
    }
}
