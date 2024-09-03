namespace StaffWebApp.Services.Staff.Requests;
public record UpdateStaffRoleRequest(Guid StaffId, Guid[] RoleIds);
