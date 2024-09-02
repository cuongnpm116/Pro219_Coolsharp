using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.GetUpdateInfo;
internal sealed class GetStaffUpdateInfoQueryHandler
    : IRequestHandler<GetStaffUpdateInfoQuery, Result>
{
    private readonly IStaffRepository _staffRepository;

    public GetStaffUpdateInfoQueryHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<Result> Handle(GetStaffUpdateInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _staffRepository.GetStaffUpdateInfo(request.StaffId, cancellationToken);
            return Result<StaffUpdateInfoVm>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
