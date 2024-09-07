namespace StaffWebApp.Services.Product.Dtos;

public record CreateProductDetailDto(
    Guid Id,
    int Stock,
    decimal Price,
    decimal OriginalPrice,
    Guid ColorId,
    Guid SizeId);