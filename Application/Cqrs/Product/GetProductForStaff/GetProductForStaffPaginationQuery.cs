using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductForStaff;
public class GetProductForStaffPaginationQuery
    : PaginationRequest, IRequest<Result<PaginationResponse<StaffAppProductVm>>>
{
    public Guid CategoryId { get; set; }
}
