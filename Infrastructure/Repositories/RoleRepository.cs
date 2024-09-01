using Application.Cqrs.Role;
using Application.Cqrs.Role.GetRoleIdsByStaffId;
using Application.IRepositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Guid>> GetRoleIdsByStaffId(GetRoleIdsByStaffIdQuery command)
    {
        var query = await _context.StaffRoles.AsNoTracking()
            .Where(sr => sr.StaffId == command.StaffId)
            .Select(sr => sr.RoleId)
            .ToListAsync();
        return query;
    }

    public async Task<IList<RoleVm>> GetRoles()
    {
        var query = await _context.Roles.AsNoTracking()
            .Select(r => new RoleVm(r.Id, r.Name))
            .ToListAsync();
        return query;
    }
}
