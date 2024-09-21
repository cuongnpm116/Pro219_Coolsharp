using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetRoleIdsByStaffId;
public record GetRoleIdsByStaffIdQuery(Guid StaffId)
    : IRequest<Result>;
