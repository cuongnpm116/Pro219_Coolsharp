using Application.Cqrs.Cart.AddCart;
using Application.Cqrs.Cart.DeleteCartItem;
using Application.Cqrs.Cart.GetCart;
using Application.Cqrs.Cart.UpdateCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetCartQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add-to-cart")]
    public async Task<IActionResult> AddToCart([FromBody] AddCartCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("update-quantity")]
    public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("delete-cart-item")]
    public async Task<IActionResult> DeleteCartItem([FromQuery] DeleteCartItemCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
