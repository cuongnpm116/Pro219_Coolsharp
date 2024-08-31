using Application.Cqrs.Product;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Common.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public Result<Dictionary<Guid, List<string>>> GetDetailImage(Guid productId)
    {
        var groupedImagePaths = (from p in _context.Products
                                 join pd in _context.ProductDetails on p.Id equals pd.ProductId
                                 join pi in _context.ProductImages on pd.Id equals pi.ProductDetailId
                                 join i in _context.Images on pi.ImageId equals i.Id
                                 where pd.ProductId == productId
                                 select new { pd.ColorId, i.ImagePath })
                                   .AsEnumerable() // phải group trong c# thay vì sql vì ef core không dịch được :/
                                   .GroupBy(x => x.ColorId)
                                   .ToDictionary(g => g.Key, g => g.Select(x => x.ImagePath).ToList());

        return Result<Dictionary<Guid, List<string>>>.Success(groupedImagePaths);
    }

    public async Task<Result<List<ProductCustomerAppVm>>> GetFeaturedProducts()
    {
        var query = from p in _context.Products
                    join pd in _context.ProductDetails on p.Id equals pd.ProductId
                    join pi in _context.ProductImages on pd.Id equals pi.ProductDetailId into ppi
                    from pi in ppi.DefaultIfEmpty()
                    join i in _context.Images on pi.ImageId equals i.Id
                    orderby p.Name descending
                    select new { p, pi, i };
        var groupedProductQuery = from product in query
                                  group product by product.p.Id into g
                                  select g.FirstOrDefault();

         var data = await groupedProductQuery.Take(8)
            .Select(x => new ProductCustomerAppVm()
            {
                ProductId = x.p.Id,
                ProductName = x.p.Name,
                ImageUrl = x.i.ImagePath
            }).ToListAsync();

        return Result<List<ProductCustomerAppVm>>.Success(data);
    }

    public async Task<Result<ProductDetailVm>> GetProductDetailForShowOnCustomerApp(Guid productId)
    {
       
        var query = from p in _context.Products.AsNoTracking()
                    join pd in _context.ProductDetails.AsNoTracking() on p.Id equals pd.ProductId
                    where pd.ProductId == productId 
                    select new { pd, p };

        Dictionary<Guid, string> colorsDictionary = [];
        Dictionary<Guid, int> sizesDictionary = [];

        var colorIds = query.Select(x => x.pd.ColorId).Distinct();
        var sizeIds = query.Select(x => x.pd.SizeId).Distinct();

        var colors = await _context.Colors.Where(c => colorIds.Contains(c.Id)).ToListAsync();
        var sizes = await _context.Sizes.Where(s => sizeIds.Contains(s.Id)).ToListAsync();

        foreach (var color in colors)
        {
            colorsDictionary.Add(color.Id, color.Name);
        }

        foreach (var size in sizes)
        {
            sizesDictionary.Add(size.Id, size.SizeNumber);
        }

        var firstQueryResult = query.FirstOrDefault() ?? throw new Exception("Không tìm thấy chi tiết sản phẩm cho sản phẩm được chỉ định.");

        var priceList = query.Select(x => x.pd.SalePrice).ToList();
        var minPrice = priceList.Min();
        var maxPrice = priceList.Max();

        var result = new ProductDetailVm
        {
            ProductId = firstQueryResult.p.Id,
            ProductName = firstQueryResult.p.Name,
            Price = minPrice,
            Stock = query.Sum(x => x.pd.Stock),
            ColorsDictionary = colorsDictionary,
            SizesDictionary = sizesDictionary
        };
        return Result<ProductDetailVm>.Success(result);
    }

    public async Task<Result<Guid>> GetProductDetailId(Guid productId, Guid colorId, Guid sizeId)
    {
        ProductDetail? detail = await _context.ProductDetails.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == productId
                   && x.ColorId == colorId && x.SizeId == sizeId);

        if (detail == null)
        {
            return Result<Guid>.Error("Không có sẵn sản phẩm này.");
        }

       
        return Result<Guid>.Success(detail.Id);
    }

    public async Task<Result<ProductPriceVm>> GetProductDetailPrice(Guid productId, Guid colorId, Guid sizeId)
    {
        var detail = await (from pd in _context.ProductDetails.AsNoTracking()
                                        
                                        where pd.ProductId == productId && pd.ColorId == colorId && pd.SizeId == sizeId
                                        select new ProductPriceVm
                                        {
                                            Price = pd.SalePrice,
                                        }).FirstOrDefaultAsync();
        if (detail == null)
        {
            return Result<ProductPriceVm>.Invalid("Không tồn tại sản phẩm.");
        }
        return Result<ProductPriceVm>.Success(detail);


    }

    public async Task<Result<int>> GetProductDetailStock(Guid productId, Guid colorId, Guid sizeId)
    {
        ProductDetail? detail = await _context.ProductDetails.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == productId
                    && x.ColorId == colorId && x.SizeId == sizeId);


        if (detail == null)
        {
            return Result<int>.Success(0);
        }

        // 200
        return Result<int>.Success(detail.Stock);
    }

    public async Task<Result<PaginationResponse<ProductCustomerAppVm>>> GetProductForShowOnCustomerApp(GetProductCustomerAppPagingQuery request)
    {
        List<Guid> productIds = new();

        if (request.CategoryIds is not null && request.CategoryIds.Count > 0)
        {
                productIds = await _context.ProductCategories
                .Where(pc => request.CategoryIds.Contains(pc.CategoryId))
                .Select(pc => pc.Product.Id)
                .Distinct()
                .ToListAsync();

        }

        var productQuery = from p in _context.Products.AsNoTracking()
                           join pd in _context.ProductDetails.AsNoTracking() on p.Id equals pd.ProductId
                           join pi in _context.ProductImages.AsNoTracking() on pd.Id equals pi.ProductDetailId into ppi
                           from pi in ppi.DefaultIfEmpty()
                           join i in _context.Images.AsNoTracking() on pi.ImageId equals i.Id into ii
                           from i in ii.DefaultIfEmpty()
                           where p.Status == Status.Active
                           select new ProductCustomerAppVm
                           {
                               ProductId = p.Id,
                               ImageUrl = i.ImagePath,
                               ProductName = p.Name,
                               Price = pd.SalePrice,
                           };


        if (productIds.Count > 0)
        {
            productQuery = productQuery.Where(p => productIds.Contains(p.ProductId));
        }

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            productQuery = productQuery.Where(p => p.ProductName.Contains(request.SearchString));
        }

        if (request.MinPrice.HasValue)
        {
            productQuery = productQuery.Where(p => p.Price >= request.MinPrice.Value);
        }
        if (request.MaxPrice.HasValue)
        {
            productQuery = productQuery.Where(p => p.Price <= request.MaxPrice.Value);
        }

        var groupedProductQuery = from product in productQuery
                                  group product by product.ProductId into g
                                  select g.FirstOrDefault();
        var result = await groupedProductQuery.ToPaginatedResponseAsync(request.PageNumber, request.PageSize);

        return Result<PaginationResponse<ProductCustomerAppVm>>.Success(result);


    }
}
