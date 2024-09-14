﻿namespace StaffWebApp.Services.Product.Requests;

public class CreateProductImageRequest
{
    public Guid ProductDetailId { get; set; }
    public Guid ImageId { get; set; }

    public CreateProductImageRequest(Guid productDetailId, Guid imageId)
    {
        ProductDetailId = productDetailId;
        ImageId = imageId;
    }
}
