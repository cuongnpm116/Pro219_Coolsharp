using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.GetUpdateInfo;
public record GetStaffUpdateInfoQuery(Guid StaffId) : IRequest<Result>;
