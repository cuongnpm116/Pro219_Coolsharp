using Application.Cqrs.Cart;
using Application.Cqrs.Order;
using Application.Cqrs.Order.CreateOrder;
using Application.IRepositories;
using Application.ValueObjects.Email;
using Common.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Repositories;
internal sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Result<OrderWithPaymentVm>> AddOrder(CreateOrderCommand request)
    {
        Order newOrder = CreateOrderFromRequest(request);
        await _context.Orders.AddAsync(newOrder);

        IReadOnlyList<OrderDetail> orderDetails = CreateOrderDetailsFromCarts(request.Carts, newOrder.Id);
        await _context.OrderDetails.AddRangeAsync(orderDetails);

        UpdateCartItemsStatus(request.Carts);
        OrderWithPaymentVm orderVm = new()
        {
            OrderId = newOrder.Id,
            OrderCode = newOrder.OrderCode,
            CustomerId = newOrder.CustomerId,
        };

        return Result<OrderWithPaymentVm>.Success(orderVm);
    }
    
    private static Order CreateOrderFromRequest(CreateOrderCommand request) => new()
    {
        CustomerId = request.CustomerId,
        OrderCode = StringUtility.DateNowToString() + StringUtility.GenerateOrderCode(8),
        Note = string.Empty,
        TotalPrice = request.TotalPrice,
        OrderStatus = OrderStatus.Pending,
        ShipAddress = request.ShipAddress,
        ShipAddressDetail = request.ShipAddressDetail,
        PhoneNumber = request.PhoneNumber,
    };

    private static List<OrderDetail> CreateOrderDetailsFromCarts(
       IReadOnlyList<CartItemVm> carts,
       Guid orderId)
    {
        var orderDetails = new List<OrderDetail>();

        foreach (var item in carts)
        {
            OrderDetail orderedItem = new()
            {
                OrderId = orderId,
                ProductDetailId = item.ProductDetailId,
                Quantity = item.Quantity,
                Price = item.UnitPrice
                
            };
            orderDetails.Add(orderedItem);
        }

        return orderDetails;
    }
    private void UpdateCartItemsStatus(IReadOnlyList<CartItemVm> carts)
    {
        foreach (var item in carts)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.CartId == item.CartId && x.Id == item.CartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
        }
    }


}
