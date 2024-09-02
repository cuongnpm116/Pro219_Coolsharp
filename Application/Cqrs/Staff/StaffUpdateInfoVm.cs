namespace Application.Cqrs.Staff;
public record StaffUpdateInfoVm(Guid StaffId, string FirstName, string LastName, DateTime DateOfBirth);
