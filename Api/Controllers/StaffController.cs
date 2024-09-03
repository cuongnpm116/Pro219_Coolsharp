using Application.Cqrs.Staff.AddStaff;
using Application.Cqrs.Staff.GetListStaff;
using Application.Cqrs.Staff.GetUpdateInfo;
using Application.Cqrs.Staff.UpdateStaffInfo;
using Application.Cqrs.Staff.UpdateStaffRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list-staff-with-pagination")]
    public async Task<IActionResult> GetListStaff([FromQuery] GetListStaffQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("add-staff")]
    public async Task<IActionResult> AddStaff([FromBody] AddStaffCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-staff-update-info")]
    public async Task<IActionResult> GetStaffUpdateInfo([FromQuery] Guid staffId)
    {
        var result = await _mediator.Send(new GetStaffUpdateInfoQuery(staffId));
        return Ok(result);
    }

    [HttpPut("update-staff-info")]
    public async Task<IActionResult> UpdateStaffInfo([FromBody] UpdateStaffInfoCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("update-staff-role")]
    public async Task<IActionResult> UpdateStaffRole([FromBody] UpdateStaffRoleCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
