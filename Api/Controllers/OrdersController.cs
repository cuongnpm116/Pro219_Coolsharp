using Application.Cqrs.Order.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
