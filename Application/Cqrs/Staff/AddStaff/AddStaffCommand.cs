using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.AddStaff;
public record AddStaffCommand(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Email,
    string Username,
    string Password,
    IReadOnlyList<Guid> RoleIds
    ) : IRequest<Result>;
