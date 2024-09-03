using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetWithPagination;
internal sealed class GetRolesWithPaginationQueryHandler
    : IRequestHandler<GetRolesWithPaginationQuery, Result>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesWithPaginationQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(GetRolesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _roleRepository.GetRolesWithPagination(request);
            return Result<PaginationResponse<RoleVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
