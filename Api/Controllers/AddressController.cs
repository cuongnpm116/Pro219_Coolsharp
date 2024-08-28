using Application.Cqrs.Address.GetAddresses;
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
}
