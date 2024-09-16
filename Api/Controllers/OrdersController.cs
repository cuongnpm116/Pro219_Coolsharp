using Application.Cqrs.Order.CancelOrder;
using Application.Cqrs.Order.CreateOrder;
using Application.Cqrs.Order.Get;
using Application.Cqrs.Order.GetById;
using Application.Cqrs.Order.GetOrderDetailForCustomer;
using Application.Cqrs.Order.GetOrderForCustomer;
using Application.Cqrs.Order.Statisticals;
using Application.Cqrs.Order.UpdateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

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
    [HttpGet("get-orders-for-customer")]
    public async Task<IActionResult> GetOrdersForCustomer([FromQuery] GetOrderForCustomerQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("get-orderdetails-for-customer")]
    public async Task<IActionResult> GetOrderDetailsForCustomer([FromQuery] Guid orderId)
    {
        GetOrderDetailForCustomerQuery query = new();
        query.OrderId = orderId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("get-orders-for-Staff")]
    public async Task<IActionResult> GetOrdersForStaff([FromQuery] GetOrdersForStaffQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("get-order-detail-staff")]
    public async Task<IActionResult> GetOrdereDetail([FromQuery] Guid orderId)
    {
        GetOrderFroStaffByIdQuery query = new(orderId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPut("update-order-status-staff")]
    public async Task<IActionResult> UpdateOrderForStaff(UpdateOrderForStaffCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    [HttpPut("cancel-order")]
    public async Task<IActionResult> CancelOrderForStaff([FromBody] CancelOrderCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }



    [HttpGet("statistical")]
    public async Task<IActionResult> TimeBasedData([FromQuery] StatisticalQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> TopProducts([FromQuery] TopProductQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }



}
