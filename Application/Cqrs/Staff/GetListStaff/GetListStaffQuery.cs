using Application.ValueObjects.Pagination;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.GetListStaff;
public class GetListStaffQuery : PaginationRequest, IRequest<Result>
{
    public Status Status { get; set; } = Status.None;
}
