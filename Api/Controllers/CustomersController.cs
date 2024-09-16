using Application.Cqrs.Customer.GetCustomer;
using Application.Cqrs.Customer.UpdateAvatar;
using Application.Cqrs.Customer.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-customer-by-id")]
    public async Task<IActionResult> Get([FromQuery] Guid customerId)
    {
        GetCustomerByIdQuery query = new();
        query.CustomerId = customerId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("update-avatar")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SaveNewAvatarImage([FromForm] UpdateAvatarCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
