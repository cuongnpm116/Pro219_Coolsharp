using Application.Cqrs.Voucher.AddVoucher;
using Application.Cqrs.Voucher.GetListVoucher;
using Application.Cqrs.Voucher.GetVoucherById;
using Application.Cqrs.Voucher.GetVoucherPaging;
using Application.Cqrs.Voucher.UpdateVoucher;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VouchersController : ControllerBase
{
    private readonly IMediator _mediator;
    public VouchersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-voucher")]
    public async Task<IActionResult> CreateVoucher([FromBody] AddVoucherCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-voucher-paging")]
    public async Task<IActionResult> GetVoucherPagination([FromQuery] GetVoucherPaginationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-list-voucher")]
    public async Task<IActionResult> GetListVoucher([FromQuery] GetListVoucherQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-voucher/{Id}")]
    public async Task<IActionResult> GetVoucher(Guid Id)
    {
        GetVoucherByIdQuery query = new();
        query.Id = Id;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("update-voucher")]
    public async Task<IActionResult> GetVouchers([FromBody] UpdateVoucherCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
