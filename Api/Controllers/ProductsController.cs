﻿using Application.Cqrs.Product.AddDetail;
using Application.Cqrs.Product.AddDetailWithNewImages;
using Application.Cqrs.Product.CheckColorExistedInProduct;
using Application.Cqrs.Product.CheckUpdateDetail;
using Application.Cqrs.Product.Create;
using Application.Cqrs.Product.GetFeaturedProduct;
using Application.Cqrs.Product.GetInfo;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductDetail;
using Application.Cqrs.Product.GetProductDetailId;
using Application.Cqrs.Product.GetProductDetailPrice;
using Application.Cqrs.Product.GetProductDetailsForStaff;
using Application.Cqrs.Product.GetProductDetailStock;
using Application.Cqrs.Product.GetProductForStaff;
using Application.Cqrs.Product.GetProductImage;
using Application.Cqrs.Product.UpdateDetailWithNewImages;
using Application.Cqrs.Product.UpdateProductDetail;
using Application.Cqrs.Product.UpdateProductInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
        GetProductDetailQuery query = new()
        {
            ProductId = productId
        };
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
        GetProductImageQuery query = new()
        {
            ProductId = productId
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("featured-product")]
    public async Task<IActionResult> FeaturedProduct([FromQuery] GetFeaturedProductQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("create-product")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("get-product-info")]
    public async Task<IActionResult> GetProductInfo([FromQuery] Guid productId)
    {
        var result = await _mediator.Send(new GetProductInfoQuery(productId));
        return Ok(result);
    }

    [HttpGet("get-product-for-staff")]
    public async Task<IActionResult> GetProductForStaff([FromQuery] GetProductForStaffPaginationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpPut("update-info")]
    public async Task<IActionResult> UpdateProductInfo([FromBody] UpdateProductInfoCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [HttpGet("get-details-for-staff")]
    public async Task<IActionResult> GetProductDetailsForStaff([FromQuery] Guid productId)
    {
        GetProductDetailsForStaffQuery query = new(productId);
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpPut("update-detail")]
    public async Task<IActionResult> UpdateProductDetail([FromBody] UpdateProductDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [HttpGet("check-update-detail-exist")]
    public async Task<IActionResult> CheckUpdateDetailExist([FromQuery] CheckUpdateDetailQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpGet("check-color-existed-in-product")]
    public async Task<IActionResult> CheckColorExistedInProduct([FromQuery] CheckColorExistedInProductQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpPut("update-detail-with-new-images")]
    public async Task<IActionResult> UpdateDetailWithNewImages([FromBody] UpdateDetailWithNewImageCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [HttpPost("add-detail-with-new-images")]
    public async Task<IActionResult> AddDetailWithNewImages([FromBody] AddDetailWithNewImagesCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }

    [HttpPost("add-detail")]
    public async Task<IActionResult> AddDetail([FromBody] AddDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }
}
