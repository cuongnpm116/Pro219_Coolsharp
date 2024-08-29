using Application.Cqrs.Staff;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResponse<StaffVm>> GetListStaff(int pageNumber, int pageSize, string searchString, Status status)
    {
        var staff = _context.Staffs.AsNoTracking();
        if (status != Status.None)
        {
            staff = staff.Where(s => s.Status == status);
        }
        var staffQuery = from s in staff
                         join sr in _context.UserRoles.AsNoTracking() on s.Id equals sr.StaffId into staffRolesGroup
                         from sr in staffRolesGroup.DefaultIfEmpty()
                         join r in _context.Roles.AsNoTracking() on sr.RoleId equals r.Id into roleGroup
                         from r in roleGroup.DefaultIfEmpty()
                         select new
                         {
                             s.Id,
                             FullName = s.LastName + " " + s.FirstName,
                             s.DateOfBirth,
                             s.Email,
                             s.Username,
                             RoleName = r.Name,
                             s.Status
                         };
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            staffQuery = staffQuery.Where(s =>
                s.FullName.ToLower().Contains(searchString) ||
                s.Username.ToLower().Contains(searchString) ||
                s.Email.ToLower().Contains(searchString) ||
                (s.RoleName != null && s.RoleName.ToLower().Contains(searchString)));
        }
        var groupedStaffQuery = staffQuery
            .GroupBy(s => new { s.Id, s.FullName, s.DateOfBirth, s.Email, s.Username, s.Status })
            .Select(g => new StaffVm(
                g.Key.Id,
                g.Key.FullName,
                g.Key.DateOfBirth,
                g.Key.Email,
                g.Key.Username,
                string.Join(", ", g.Select(x => x.RoleName).Where(role => role != null)),
                g.Key.Status));

        PaginationResponse<StaffVm> response = await groupedStaffQuery.ToPaginatedResponseAsync(pageNumber, pageSize);
        return response;
    }
}
