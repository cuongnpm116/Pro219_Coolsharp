using Application.Cqrs.Product.GetFeaturedProduct;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductDetail;
using Application.Cqrs.Product.GetProductDetailId;
using Application.Cqrs.Product.GetProductDetailPrice;
using Application.Cqrs.Product.GetProductDetailStock;
using Application.Cqrs.Product.GetProductImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("get-product-paging")]
        public async Task<IActionResult> ShowProducts([FromQuery] GetProductCustomerAppPagingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("show-productDetail/{productId}")]
        public async Task<IActionResult> ShowProductDetail(Guid productId)
        {
            GetProductDetailQuery query = new GetProductDetailQuery();
            query.ProductId = productId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("get-productdetail-price")]
        public async Task<IActionResult> GetProductDetailPrice([FromQuery] GetProductDetailPriceQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("get-productdetail-stock")]
        public async Task<IActionResult> GetProductDetailStock([FromQuery] GetProductDetailStockQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("get-productdetailId")]
        public async Task<IActionResult> GetProductDetailId([FromQuery] GetProductDetailIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("detail-image")]
        public async Task<IActionResult> GetDetailImage([FromQuery] Guid productId)
        {
            GetProductImageQuery query = new();
            query.ProductId = productId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("featured-product")]
        public async Task<IActionResult> FeaturedProduct([FromQuery] GetFeaturedProductQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
