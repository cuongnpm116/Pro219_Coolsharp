using Application.Cqrs.Color.Create;
using Application.Cqrs.Color.Get;
using Application.Cqrs.Color.GetById;
using Application.Cqrs.Color.GetForSelect;
using Application.Cqrs.Color.Update;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColorsController : Controller
{
    private readonly IMediator _mediator;

    public ColorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetColors([FromQuery] GetColorsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("GetColorById")]
    public async Task<IActionResult> GetSizeById([FromQuery] Guid Id)
    {
        GetColorByIdQuery query = new(Id);
        Result result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor(CreateColorCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateColor(UpdateColorCommand request)
    {
        Result result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("color-for-select")]
    public async Task<IActionResult> GetColorForSelect()
    {
        var result = await _mediator.Send(new GetColorForSelectQuery());
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Message);
    }
}
