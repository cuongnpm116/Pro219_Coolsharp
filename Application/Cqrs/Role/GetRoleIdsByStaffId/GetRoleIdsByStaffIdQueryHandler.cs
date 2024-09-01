using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetRoleIdsByStaffId;
internal sealed class GetRoleIdsByStaffIdQueryHandler
    : IRequestHandler<GetRoleIdsByStaffIdQuery, Result>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleIdsByStaffIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(GetRoleIdsByStaffIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _roleRepository.GetRoleIdsByStaffId(request);
            return Result<IList<Guid>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
