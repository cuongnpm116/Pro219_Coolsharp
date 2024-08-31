using Application.Cqrs.Cart;
using Application.Cqrs.Cart.AddCart;
using Application.Cqrs.Cart.UpdateCart;
using Application.IRepositories;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Result<bool>> AddToCart(AddCartCommand request)
    {
        var productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.ColorId == request.ColorId && x.SizeId == request.SizeId);
        if (productDetail == null)
        {
            return Result<bool>.Invalid("Sản phẩm không có sẵn");
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);
        if (cart == null)
        {
            return Result<bool>.Invalid("Không tìm thấy giỏ hàng");
        }
        var existingCartItem = cart.CartItems
            .FirstOrDefault(ci => ci.ProductDetailId == productDetail.Id);

        int totalRequestedQuantity = request.Quantity;

        if (existingCartItem != null)
        {
            totalRequestedQuantity += existingCartItem.Quantity;
        }

        if (totalRequestedQuantity > productDetail.Stock)
        {
            return Result<bool>.Invalid("Số lượng yêu cầu vượt quá số lượng tồn kho");
        }

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.Quantity;

            _context.CartItems.Update(existingCartItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductDetailId = productDetail.Id,
                Quantity = request.Quantity,
            };

            _context.CartItems.Add(cartItem);
        }
        // Trả về kết quả thành công
        return Result<bool>.Success(true);
    }
    public async Task<Result<CartVm>> GetCart(Guid customerId)
    {
        var query = from c in _context.Carts
                    join ci in _context.CartItems on c.Id equals ci.CartId
                    join pd in _context.ProductDetails on ci.ProductDetailId equals pd.Id
                    join p in _context.Products on pd.ProductId equals p.Id
                    join pi in _context.ProductImages on pd.Id equals pi.ProductDetailId into piGroup
                    from pi in piGroup.DefaultIfEmpty()
                    join i in _context.Images on pi.ImageId equals i.Id into piImage
                    from i in piImage.DefaultIfEmpty()
                    join cl in _context.Colors on pd.ColorId equals cl.Id
                    join s in _context.Sizes on pd.SizeId equals s.Id
                    where c.CustomerId == customerId 
                    select new CartItemVm
                    {
                        CartId = c.Id,
                        CartItemId = ci.Id,
                        ProductQuantity = pd.Stock,
                        ProductId = p.Id,
                        ProductDetailId = pd.Id,
                        ProductName = p.Name,
                        SizeNumber = s.SizeNumber,
                        ColorName = cl.Name,
                        ImagePath = i.ImagePath,
                        Quantity = ci.Quantity,
                        UnitPrice = pd.SalePrice,
                        AmountOfMoney = ci.Quantity * pd.SalePrice
                    };


        var listCartItem = await query.AsNoTracking().ToListAsync();

        CartVm cartVm = new()
        {
            CustomerId = customerId,
            ListCart = listCartItem,
        };

        return Result<CartVm>.Success(cartVm);
    }

    public async Task<Result<bool>> DeleteCartItem(List<Guid> productDetailIds)
    {
        var listCartItem = await _context.CartItems.ToListAsync();
        if (!listCartItem.Any())
        {
            return Result<bool>.Invalid("Yêu cầu không hợp lệ");
        }
        List<CartItem> lstForDelete = new();
        foreach (var item in productDetailIds)
        {
            var objForDelete = listCartItem.FirstOrDefault(x => x.ProductDetailId == item);
            if (objForDelete != null)
                lstForDelete.Add(objForDelete);
        }
        _context.CartItems.RemoveRange(lstForDelete);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateCartItemQuantity(UpdateCartCommand request)
    {
        var cartItem = await _context.CartItems
               .FirstOrDefaultAsync(ci => ci.CartId == request.CartId && ci.ProductDetailId == request.ProductDetailId);

        if (cartItem == null)
        {
            return Result<bool>.Invalid("Không tồn tại sản phẩm trong giỏ hàng");
        }

        cartItem.Quantity = request.Quantity;

        _context.CartItems.Update(cartItem);

        return Result<bool>.Success(true);
    }
}
