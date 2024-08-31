using Application.Cqrs.Category.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
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
    }
}
