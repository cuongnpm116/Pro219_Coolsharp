using Application.Cqrs.Customer.Authenticate;
using Application.Cqrs.Customer.ChangePassword;
using Application.Cqrs.Customer.Create;
using Application.Cqrs.Customer.ForgetPassword;
using Application.Cqrs.Customer.GetCustomer;
using Application.Cqrs.Customer.GetCustomerWithPagination;
using Application.Cqrs.Customer.UniqueEmail;
using Application.Cqrs.Customer.UniqueUsername;
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
    [HttpGet("list-customer")]
    public async Task<IActionResult> GetListCustomer([FromQuery] GetCustomerWithPaginationQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    [HttpGet("get-customer-by-id")]
    public async Task<IActionResult> Get([FromQuery] Guid customerId)
    {
        GetCustomerByIdQuery query = new();
        query.CustomerId = customerId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create-customer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateUserCommand request)
    {
        var result = await _mediator.Send(request);
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


    [HttpGet("check-unique-email")]
    public async Task<IActionResult> CheckUniqueEmail([FromQuery] string email)
    {
        CheckEmailCommand command = new();
        command.EmailAddress = email;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("check-unique-username")]
    public async Task<IActionResult> CheckUniqueUsername([FromQuery] string username)
    {
        CheckUsernameCommand command = new();
        command.Username = username;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromQuery] string userInput)
    {
        ForgetPasswordCommand command = new();
        command.UserInput = userInput;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("login-customer")]
    public async Task<IActionResult> LoginCustomer(AuthenticateCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

}
