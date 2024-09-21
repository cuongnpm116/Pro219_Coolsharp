using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UnitTest;

public class SearchTest
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    public SearchTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "InMemoryProductDb")
                            .Options;

        SeedDatabase();
    }
    private void SeedDatabase()
    {
        using var context = new AppDbContext(_dbContextOptions);

        // Tạo dữ liệu mẫu cho Product, ProductDetail, ProductImage, Image, ProductCategory
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Adidas Shoes",
            Status = Status.Active
        };

        context.Products.Add(product);


        var size = new Size
        {
            Id = Guid.NewGuid(),
            SizeNumber = 40,
            Status = Status.Active
        };
        context.Sizes.Add(size);


        var color = new Color
        {
            Id = Guid.NewGuid(),
            Name = "Đen",
            Status = Status.Active
        };
        context.Colors.Add(color);

        var image = new Image
        {
            Id = Guid.NewGuid(),
            ImagePath = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/15f901c90a9549d29104aae700d27efb_9366/Giay_Superstar_DJen_EG4959_01_standard.jpg"
        };
        context.Images.Add(image);

        var productDetail = new ProductDetail
        {
            Id = Guid.NewGuid(),
            Stock = 40,
            SalePrice = 1000000,
            OriginalPrice = 500000,
            ProductId = product.Id,
            ColorId = color.Id,
            SizeId = size.Id
        };
        context.ProductDetails.Add(productDetail);

        var productImage = new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductDetailId = productDetail.Id,
            ImageId = image.Id,
        };
        context.ProductImages.Add(productImage);

        context.SaveChanges();
    }
    [Fact]
    public async Task GetProductForShowOnCustomerApp_ShouldReturnFilteredProductsBySearchString()
    {
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new ProductRepository(context);
        var request = new GetProductCustomerAppPagingQuery
        {
            SearchString = "Adidas",
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await repository.GetProductForShowOnCustomerApp(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Value.Data.Any());
        Assert.Equal("Adidas Shoes", result.Value.Data.First().ProductName);

    }
    [Fact]
    public async Task GetProductForShowOnCustomerApp_ShouldReturnEmptyIfNoMatch()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new ProductRepository(context);

        var request = new GetProductCustomerAppPagingQuery
        {
            SearchString = "NonExistingProduct",
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await repository.GetProductForShowOnCustomerApp(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Value.Data.Any());
    }
}