using WebAppIntegrated.Enum;

namespace StaffWebApp.Components.Staff.Vms;
public record StaffVm(
    Guid Id,
    string FullName,
    DateTime Dob,
    string Email,
    string Username,
    string Roles,
    Status Status);
