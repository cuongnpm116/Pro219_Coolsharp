using Application.Cqrs.Product;
using Application.Cqrs.Product.Create;
using Application.Cqrs.Product.GetInfo;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductDetailsForStaff;
using Application.Cqrs.Product.GetProductForStaff;
using Application.Cqrs.Product.UpdateProductDetail;
using Application.Cqrs.Product.UpdateProductInfo;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateProductAsync(CreateProductCommand request)
    {
        Product newProduct = new(request.Name);
        await _context.Products.AddAsync(newProduct);
        await _context.ProductCategories.AddRangeAsync(
            request.CategoryIds.Select(x => new ProductCategory(x, newProduct.Id)));
        await _context.Images.AddRangeAsync(request.Images.Select(x => new Image(x.Id, x.Path)));
        await _context.ProductDetails.AddRangeAsync(
            request.Details.Select(x => new ProductDetail(
                x.Id,
                newProduct.Id,
                x.SizeId,
                x.ColorId,
                x.Stock,
                x.Price,
                x.OriginalPrice)));
        await _context.ProductImages.AddRangeAsync(
            request.DetailImages.Select(x => new ProductImage(x.ProductDetailId, x.ImageId)));
        return true;
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

    public async Task<Result<ProductPriceVm>> GetProductDetailPrice(
        Guid productId,
        Guid colorId,
        Guid sizeId)
    {
        var detail = await (from pd in _context.ProductDetails.AsNoTracking()
                            where pd.ProductId == productId
                            && pd.ColorId == colorId
                            && pd.SizeId == sizeId
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
        List<Guid> productIds = [];

        if (request.CategoryIds is not null && request.CategoryIds.Count > 0)
        {
            productIds = await _context.ProductCategories
            .Where(pc => request.CategoryIds.Contains(pc.CategoryId))
            .Select(pc => pc.Product.Id)
            .Distinct()
            .ToListAsync();

        }

        var productQuery = from p in _context.Products.AsNoTracking()
                           join pd in _context.ProductDetails.AsNoTracking()
                           on p.Id equals pd.ProductId
                           join pi in _context.ProductImages.AsNoTracking()
                           on pd.Id equals pi.ProductDetailId into ppi
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
        var result = await groupedProductQuery
            .ToPaginatedResponseAsync(request.PageNumber, request.PageSize);

        return Result<PaginationResponse<ProductCustomerAppVm>>.Success(result);
    }

    public async Task<ProductInfoDto> GetProductInfo(Guid productId)
    {
        ProductInfoDto result = new()
        {
            Name = await _context.Products.AsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => x.Name)
                .SingleOrDefaultAsync(),
            CategoryIds = _context.ProductCategories.AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => x.CategoryId)
                .AsEnumerable()
        };
        return result;
    }

    public async Task<PaginationResponse<StaffAppProductVm>> GetStaffAppProducts(
        GetProductForStaffPaginationQuery request)
    {
        var query = from p in _context.Products
                    join pd in _context.ProductDetails on p.Id equals pd.ProductId
                    where p.Status == Status.Active
                    group pd by new { p.Id, p.Name, p.Status } into g
                    select new StaffAppProductVm
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        TotalStock = g.Sum(x => x.Stock),
                    };

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            query = query.Where(x => x.Name.Contains(request.SearchString));
        }

        if (request.CategoryId != Guid.Empty)
        {
            query = from q in query
                    join pc in _context.ProductCategories on q.Id equals pc.ProductId
                    where pc.CategoryId == request.CategoryId
                    select q;
        }

        var products = await query.ToPaginatedResponseAsync(request.PageNumber, request.PageSize);

        IEnumerable<Guid> productIds = products.Data.Select(x => x.Id);
        var categories = await (from pc in _context.ProductCategories
                                join c in _context.Categories on pc.CategoryId equals c.Id
                                where productIds.Contains(pc.ProductId)
                                select new
                                {
                                    pc.ProductId,
                                    c.Name
                                }).ToListAsync();

        foreach (var product in products.Data)
        {
            product.Categories = categories
                                .Where(x => x.ProductId == product.Id)
                                .Select(x => x.Name);
        }

        return products;
    }

    public async Task<bool> UpdateProductInfoAsync(UpdateProductInfoCommand request)
    {
        Product? product = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id);
        if (product == null)
        {
            return false;
        }

        product.Name = request.Name;
        _context.Products.Update(product);

        var currentCategoryIds = _context.ProductCategories
            .Where(x => x.ProductId == request.Id)
            .Select(x => x.CategoryId)
            .ToHashSet();

        var toAdd = request.CategoryIds
            .Where(x => !currentCategoryIds.Contains(x))
            .ToList();

        var toRemove = currentCategoryIds
            .Where(x => !request.CategoryIds.Contains(x))
            .ToList();

        if (toRemove.Count > 0)
        {
            var categoriesToRemove = _context.ProductCategories
                .Where(x => toRemove.Contains(x.CategoryId) && x.ProductId == request.Id);
            _context.ProductCategories.RemoveRange(categoriesToRemove);
        }

        if (toAdd.Count > 0)
        {
            var newProductCategories = toAdd.Select(x => new ProductCategory(x, request.Id));
            await _context.ProductCategories.AddRangeAsync(newProductCategories);
        }
        return true;
    }

    public async Task<IEnumerable<ProductDetailForStaffVm>> GetProductDetailsForStaff(Guid productId)
    {
        var productDetails = await _context.ProductDetails
            .Where(pd => pd.ProductId == productId)
            .Select(pd => new ProductDetailForStaffVm
            {
                Id = pd.Id,
                Stock = pd.Stock,
                OriginalPrice = pd.OriginalPrice,
                Price = pd.SalePrice,
                Color = new(pd.Color.Id, pd.Color.Name),
                Size = new(pd.Size.Id, pd.Size.SizeNumber),
            })
            .ToListAsync();
        return productDetails;
    }

    public async Task<bool> UpdateProductDetail(UpdateProductDetailCommand request)
    {
        var productDetail = await _context.ProductDetails.FindAsync(request.Id);

        if (productDetail == null)
        {
            return false;
        }

        productDetail.Stock = request.Stock;
        productDetail.OriginalPrice = request.OriginalPrice;
        productDetail.SalePrice = request.Price;
        productDetail.ColorId = request.ColorId;
        productDetail.SizeId = request.SizeId;

        _context.ProductDetails.Update(productDetail);

        return true;
    }
}

