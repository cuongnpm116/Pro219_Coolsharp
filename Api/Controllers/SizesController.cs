using Application.Cqrs.Size.Create;
using Application.Cqrs.Size.Get;
using Application.Cqrs.Size.GetById;
using Application.Cqrs.Size.GetForSelect;
using Application.Cqrs.Size.Update;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SizesController : Controller
{
    private readonly IMediator _mediator;

    public SizesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSizes([FromQuery] GetSizesQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("GetSizeById")]
    public async Task<IActionResult> GetSizeById([FromQuery] Guid Id)
    {
        GetSizeByIdQuery query = new(Id);
        Result result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSize(CreateSizeCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSize(UpdateSizeCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("size-for-select")]
    public async Task<IActionResult> GetSizeForSelect()
    {
        var result = await _mediator.Send(new GetSizeForSelectQuery());
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Message);
    }
}
