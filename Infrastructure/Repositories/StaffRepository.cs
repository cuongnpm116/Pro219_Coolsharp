using Application.Cqrs.Staff;
using Application.Cqrs.Staff.AddStaff;
using Application.Cqrs.Staff.GetListStaff;
using Application.Cqrs.Staff.UpdateStaffInfo;
using Application.Cqrs.Staff.UpdateStaffRole;
using Application.IRepositories;
using Application.IServices;
using Application.ValueObjects.Pagination;
using Common.Consts;
using Common.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Repositories;
internal sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public StaffRepository(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<bool> AddStaffAsync(AddStaffCommand command)
    {
        var staff = new Staff
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            DateOfBirth = command.DateOfBirth,
            Email = command.Email,
            Username = command.Username,
            HashedPassword = HashUtility.Hash(command.Password),
            Status = Status.Active
        };
        await _context.Staffs.AddAsync(staff);

        StaffRole[] staffRoles = new StaffRole[command.RoleIds.Count];
        for (int i = 0; i < command.RoleIds.Count; i++)
        {
            staffRoles[i] = new StaffRole
            {
                StaffId = staff.Id,
                RoleId = command.RoleIds[i]
            };
        }
        await _context.StaffRoles.AddRangeAsync(staffRoles);

        return true;
    }

    public async Task<PaginationResponse<StaffVm>> GetListStaff(GetListStaffQuery query)
    {
        var staff = _context.Staffs.AsNoTracking();
        if (query.Status != Status.None)
        {
            staff = staff.Where(s => s.Status == query.Status);
        }
        var staffQuery = from s in staff
                         join sr in _context.StaffRoles.AsNoTracking() on s.Id equals sr.StaffId into staffRolesGroup
                         from sr in staffRolesGroup.DefaultIfEmpty()
                         join r in _context.Roles.AsNoTracking() on sr.RoleId equals r.Id into roleGroup
                         from r in roleGroup.DefaultIfEmpty()
                         select new
                         {
                             s.Id,
                             // không dùng được $"" trong linq vì sql không hỗ trợ
                             FullName = s.LastName + " " + s.FirstName,
                             s.DateOfBirth,
                             s.Email,
                             s.Username,
                             RoleName = r.Name,
                             s.Status
                         };
        if (!string.IsNullOrEmpty(query.SearchString))
        {
            staffQuery = staffQuery.Where(s =>
                s.FullName.ToLower().Contains(query.SearchString) ||
                s.Username.ToLower().Contains(query.SearchString) ||
                s.Email.ToLower().Contains(query.SearchString) ||
                (s.RoleName != null && s.RoleName.ToLower().Contains(query.SearchString)));
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

        PaginationResponse<StaffVm> response = await groupedStaffQuery.ToPaginatedResponseAsync(query.PageNumber, query.PageSize);
        return response;
    }

    public async Task<bool> UpdateStaffInfo(UpdateStaffInfoCommand command, CancellationToken cancellationToken)
    {
        var exist = await _context.Staffs.FirstOrDefaultAsync(s => s.Id == command.StaffId, cancellationToken);
        if (exist is null)
        {
            return false;
        }
        exist.UpdateInfo(command.FirstName, command.LastName, command.DateOfBirth);
        _context.Staffs.Update(exist);
        return true;
    }

    public async Task<StaffUpdateInfoVm> GetStaffUpdateInfo(Guid staffId, CancellationToken cancellationToken)
    {
        var vm = await _context.Staffs
            .AsNoTracking()
            .Where(s => s.Id == staffId)
            .Select(s => new StaffUpdateInfoVm(s.Id, s.FirstName, s.LastName, s.DateOfBirth,s.Username, s.ImageUrl))
            .SingleOrDefaultAsync(cancellationToken);
        return vm;
    }

    public async Task<bool> UpdateStaffRole(UpdateStaffRoleCommand command)
    {
        var staffRoles = await _context.StaffRoles
            .Where(sr => sr.StaffId == command.StaffId)
            .ToListAsync();
        var toRemove = staffRoles.Where(sr => !command.RoleIds.Contains(sr.RoleId));
        var toAdd = command.RoleIds.Where(roleId => !staffRoles.Any(sr => sr.RoleId == roleId))
            .Select(roleId => new StaffRole
            {
                StaffId = command.StaffId,
                RoleId = roleId
            });
        _context.StaffRoles.RemoveRange(toRemove);
        await _context.StaffRoles.AddRangeAsync(toAdd);
        return true;
    }

    public  async Task<Result<string>> Authenticate(string username, string password)
    {
        Staff? exist = await _context.Staffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);
        if (exist == null)
        {
            return Result<string>.Invalid("Tên tài khoản không tồn tại. Vui lòng nhập lại tên tài khoản");
        }

        if (!HashUtility.Verify(password, exist.HashedPassword))
        {
            return Result<string>.Invalid("Mật khẩu không chính xác. Vui lòng nhập lại mật khẩu");
        }

        List<string> roleNames = await (from r in _context.Roles
                                        join ur in _context.StaffRoles on r.Id equals ur.RoleId
                                        where ur.StaffId == exist.Id
                                        select r.Code)
                                        .ToListAsync();

        Claim[] claims =
        [
                new("UserId", exist.Id.ToString()),
                new(ClaimTypes.Name, exist.Username),
                new(ClaimTypes.Email, exist.Email),
                new(ClaimTypes.Role, string.Join(";", roleNames)),
            ];

        string token = _tokenService.GenerateToken(claims);

        return Result<string>.Success(token);
    }
}
