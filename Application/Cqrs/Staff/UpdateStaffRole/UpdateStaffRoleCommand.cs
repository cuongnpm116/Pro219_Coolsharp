using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.UpdateStaffRole;
public record UpdateStaffRoleCommand(Guid StaffId, Guid[] RoleIds)
    : IRequest<Result>;
