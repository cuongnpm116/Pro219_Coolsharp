using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.GetListStaff;
internal sealed class GetListStaffQueryHandler : IRequestHandler<GetListStaffQuery, Result>
{
    private readonly IStaffRepository _staffRepo;

    public GetListStaffQueryHandler(IStaffRepository staffRepo)
    {
        _staffRepo = staffRepo;
    }

    public async Task<Result> Handle(GetListStaffQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _staffRepo.GetListStaff(request.PageNumber, request.PageSize, request.SearchString, request.Status);
            return Result<PaginationResponse<StaffVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
