using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.UpdateStaffInfo;
public record UpdateStaffInfoCommand(
    Guid StaffId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth
    ) : IRequest<Result>;
