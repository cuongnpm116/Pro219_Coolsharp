using Application.Cqrs.Product;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductStaffPaging;
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
public  class ProductRepository : IProductRepository
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

    public Result<List<ProductCustomerAppVm>> GetFeaturedProducts()
    {
        var query = from p in _context.Products
                    join pd in _context.ProductDetails on p.Id equals pd.ProductId
                    join pi in _context.ProductImages on pd.Id equals pi.ProductDetailId into ppi
                    from pi in ppi.DefaultIfEmpty()
                    join i in _context.Images on pi.ImageId equals i.Id into ii
                    from i in ii.DefaultIfEmpty()
                    orderby p.Name descending
                    select new { p.Id, p.Name, i.ImagePath };

        
        var groupedProductQuery = query.AsEnumerable()
                                       .GroupBy(product => product.Id)
                                       .Select(g => g.FirstOrDefault());

        var data = groupedProductQuery.Take(8)
                                      .Select(x => new ProductCustomerAppVm()
                                      {
                                          ProductId = x.Id,
                                          ProductName = x.Name,
                                          ImageUrl = x.ImagePath
                                      }).ToList();

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

    public async Task<Result<PaginationResponse<ProductStaffVm>>> GetProductForStaffView(GetProductStaffPagingQuery request)
    {

        var productsQuery = _context.Products
            .Include(x=>x.ProductDetails)
            .ThenInclude(x=>x.Color)
            .Include(x=>x.ProductDetails)
            .ThenInclude(x=>x.Size)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            productsQuery = productsQuery.Where(x =>
                _context.ProductCategories
                    .Where(pc => pc.ProductId == x.Id)
                    .Join(_context.Categories,
                          pc => pc.CategoryId,
                          c => c.Id,
                          (pc, c) => c.Name)
                    .Any(cName => cName.ToLower().Trim() == request.CategoryName.ToLower().Trim())
            );
        }

        
        var productDetailsQuery = _context.ProductDetails.AsQueryable();
        var totalStock = productDetailsQuery.Sum(pd => pd.Stock);
        var minPrice = productDetailsQuery.Min(pd => pd.SalePrice);

       var productQuery = productsQuery
            .Select(x => new ProductStaffVm
            {
                Id = x.Id,
                Name = x.Name,
                TotalStock = totalStock,
                MinPrice = minPrice,
                Status = x.Status,
                ProductDetails = (from pd in _context.ProductDetails
                                  join c in _context.Colors on pd.ColorId equals c.Id
                                  join s in _context.Sizes on pd.SizeId equals s.Id
                                  join pi in _context.ProductImages on pd.Id equals pi.ProductDetailId into ppi
                                  from pi in ppi.DefaultIfEmpty()
                                  join i in _context.Images on pi.ImageId equals i.Id into ii
                                  from i in ii.DefaultIfEmpty()
                                  where pd.ProductId == x.Id
                                  group i by new { pd.Id, pd.Stock, pd.SalePrice, pd.OriginalPrice, c.Name, s.SizeNumber } into g
                                  select new ProductDetailStaffVm
                                  {
                                      Id = g.Key.Id,
                                      Stock = g.Key.Stock,
                                      Price = g.Key.SalePrice,
                                      OriginalPrice = g.Key.OriginalPrice,
                                      Color = g.Key.Name,
                                      Size = g.Key.SizeNumber,
                                      Images = g.Select(img => img.ImagePath).Where(path => path != null).ToList()

                                  }).ToList()
            });

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower().Trim();

            // Lọc products theo SearchString
            productQuery = productQuery.Where(p =>
                p.Name.ToLower().Trim().Contains(searchString) ||
                p.ProductDetails.Any(d =>
                    d.Color.ToLower().Trim().Contains(searchString) ||
                    d.Size.ToString().Contains(searchString))
            );
        }

        // Sau cùng gọi ToListAsync() để lấy kết quả cuối cùng
        
        var groupedProductQuery = from product in productQuery
                                  group product by product.Id into g
                                  select g.FirstOrDefault();
        var result = await groupedProductQuery.ToPaginatedResponseAsync(request.PageNumber, request.PageSize);

        return Result<PaginationResponse<ProductStaffVm>>.Success(result);
    }
}
