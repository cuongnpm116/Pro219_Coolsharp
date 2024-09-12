using Application.Cqrs.Category.CheckExist;
using Application.Cqrs.Category.Create;
using Application.Cqrs.Category.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetListCategoryQuery categoriesQuery)
    {
        var result = await _mediator.Send(categoriesQuery);
        return Ok(result);
    }

    [HttpGet("check-exist")]
    public async Task<IActionResult> CheckExistCategoryName([FromQuery] string name)
    {
        var result = await _mediator.Send(new CheckExistCategoryNameQuery(name));
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody] string name)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(name));
        return Ok(result);
    }
}
