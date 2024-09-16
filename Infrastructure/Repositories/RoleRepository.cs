using Application.Cqrs.Role.GetAll;
using Application.Cqrs.Role.GetRoleIdsByStaffId;
using Application.Cqrs.Role.GetWithPagination;
using Application.Cqrs.Role.Update;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Guid>> GetRoleIdsByStaffId(GetRoleIdsByStaffIdQuery command)
    {
        var query = await _context.StaffRoles.AsNoTracking()
            .Where(sr => sr.StaffId == command.StaffId)
            .Select(sr => sr.RoleId)
            .ToListAsync();
        return query;
    }

    public async Task<IReadOnlyList<RoleVmForGetAll>> GetAllRoles()
    {
        var query = await _context.Roles.AsNoTracking()
            .Select(r => new RoleVmForGetAll(r.Id, r.Name))
            .ToListAsync();
        return query;
    }

    public async Task<PaginationResponse<RoleVm>> GetRolesWithPagination(GetRolesWithPaginationQuery query)
    {
        var roleQueryable = _context.Roles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.SearchString))
        {
            roleQueryable = roleQueryable
                .Where(x => x.Name.Contains(query.SearchString, StringComparison.CurrentCultureIgnoreCase)
                || x.Code.Contains(query.SearchString, StringComparison.CurrentCultureIgnoreCase));
        }
        var result = await roleQueryable.Select(x => new RoleVm(x.Id, x.Code, x.Name))
                .ToPaginatedResponseAsync(query.PageNumber, query.PageSize);
        return result;
    }

    public async Task<bool> UpdateRole(UpdateRoleCommand command)
    {
        var exist = await _context.Roles.SingleOrDefaultAsync(x => x.Id == command.Id);
        if (exist is null)
        {
            return false;
        }

        exist.Code = command.Code;
        exist.Name = command.Name;
        _context.Roles.Update(exist);
        return true;
    }

    public async Task<bool> CreateRole(string code, string name)
    {
        await _context.Roles.AddAsync(new(code, name));
        return true;
    }

    public async Task<bool> IsUniqueCode(string code)
    {
        return await _context.Roles.AnyAsync(x => x.Code == code);
    }

    public async Task<bool> IsUniqueName(string name)
    {
        return await _context.Roles.AnyAsync(x => x.Name == name);
    }
}
