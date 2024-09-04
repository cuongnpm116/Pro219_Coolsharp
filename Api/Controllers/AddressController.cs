using Application.Cqrs.Address.AddAddress;
using Application.Cqrs.Address.DeleteAddress;
using Application.Cqrs.Address.GetAddressById;
using Application.Cqrs.Address.GetAddresses;
using Application.Cqrs.Address.GetDefaultAddress;
using Application.Cqrs.Address.MakeDefaultAddress;
using Application.Cqrs.Address.UpdateAddress;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-addresses")]
    public async Task<IActionResult> GetAddresses([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetAddressesQuery(userId));
        return Ok(result);
    }

    [HttpPost("add-address")]
    public async Task<IActionResult> AddUserAddress([FromBody] AddCustomerAddressCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("update-address")]
    public async Task<IActionResult> UpdateUserAddress([FromBody] UpdateCustomerAddressCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-address-by-id")]
    public async Task<IActionResult> GetAddressById([FromQuery] Guid addressId)
    {
        GetCustomerAddressByIdQuery command = new(addressId);
        Result result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("make-default-address")]
    public async Task<IActionResult> MakeDefaultAddress([FromBody] MakeDefaultAddressCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("delete-address")]
    public async Task<IActionResult> DeleteAddress([FromBody] DeleteAddressCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("default-address")]
    public async Task<IActionResult> GetDefaultAddress([FromQuery] Guid userId)
    {
        GetDefaultAddressQuery query = new(userId);
        Result result = await _mediator.Send(query);
        return Ok(result);
    }
}
