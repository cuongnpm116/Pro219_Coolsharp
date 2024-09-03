using Application.Cqrs.Role.GetRoleIdsByStaffId;
using Application.Cqrs.Role.GetRoles;
using Application.Cqrs.Role.GetWithPagination;
using Application.Cqrs.Role.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    [HttpGet("get-roleids-by-staff-id")]
    public async Task<IActionResult> GetRoleIdsByStaffId([FromQuery] Guid staffId)
    {
        var result = await _mediator.Send(new GetRoleIdsByStaffIdQuery(staffId));
        return Ok(result);
    }

    [HttpGet("get-roles-with-pagination")]
    public async Task<IActionResult> GetRolesWithPagination([FromQuery] GetRolesWithPaginationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("update-role")]
    public async Task<IActionResult> UpdateRole(UpdateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
