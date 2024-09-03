using Application.Cqrs.Cart;
using Application.Cqrs.Order;
using Application.Cqrs.Order.CancelOrder;
using Application.Cqrs.Order.CreateOrder;
using Application.Cqrs.Order.Get;
using Application.Cqrs.Order.UpdateOrder;
using Application.Cqrs.Payment;
using Application.IRepositories;
using Application.ValueObjects.Email;
using Application.ValueObjects.Pagination;
using Common.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using MailKit.Search;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Application.IServices;
using System.Text;

namespace Infrastructure.Repositories;
internal sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    public OrderRepository(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }
    public async Task<Result<OrderWithPaymentVm>> AddOrder(CreateOrderCommand request)
    {
        Order newOrder = CreateOrderFromRequest(request);
        await _context.Orders.AddAsync(newOrder);

        IReadOnlyList<OrderDetail> orderDetails = CreateOrderDetailsFromCarts(request.Carts, newOrder.Id);
        await _context.OrderDetails.AddRangeAsync(orderDetails);

        UpdateCartItems(request.Carts);
        if (request.VoucherId != null)
        {
            UpdateVoucher(request.VoucherId.Value);
        }
        OrderWithPaymentVm orderVm = new()
        {
            OrderId = newOrder.Id,
            OrderCode = newOrder.OrderCode,
            CustomerId = newOrder.CustomerId,
        };

        return Result<OrderWithPaymentVm>.Success(orderVm);
    }
    private void UpdateVoucher(Guid voucherId)
    {

        var voucher = _context.Vouchers
            .FirstOrDefault(x => x.Id == voucherId);
        if (voucher != null)
        {
            voucher.Stock -= 1;
            _context.Vouchers.Update(voucher);
        }

    }
    private static Order CreateOrderFromRequest(CreateOrderCommand request) => new()
    {
        CustomerId = request.CustomerId,
        VoucherId = request.VoucherId,
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
    private void UpdateCartItems(IReadOnlyList<CartItemVm> carts)
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




    #region OrderStaff
    public async Task<Result<PaginationResponse<OrderVm>>> GetOrdersForStaff(GetOrdersForStaffQuery request)
    {
        var query = from a in _context.Orders
                    join b in _context.OrderDetails on a.Id equals b.OrderId into orderGroup
                    join c in _context.Payments on a.Id equals c.OrderId
                    orderby a.CreatedOn descending
                    select new OrderVm
                    {
                        Id = a.Id,
                        OrderCode = a.OrderCode,
                        CreatedOn = a.CreatedOn,
                        ConfirmedDate = a.ConfirmedDate,
                        ShippedDate = a.ShippedDate,
                        CompletedDate = a.CompletedDate,
                        PaymentStatus = c.Status,
                        PaymentMethod = c.PaymentMethod,
                        TotalPrice = orderGroup.Sum(d => d.Price * d.Quantity),
                        OrderStatus = a.OrderStatus,
                        Note = a.Note,
                        StaffId = a.StaffId,
                        CustomerId = a.CustomerId,
                        PhoneNumber = a.PhoneNumber,
                        ShipAddress = a.ShipAddress,
                        ShipAddressDetail = a.ShipAddressDetail,
                    };

        // Lọc theo Status
        if (!string.IsNullOrWhiteSpace(request.OrderStatus.ToString()) && request.OrderStatus != OrderStatus.None)
        {
            query = query.Where(s => s.OrderStatus == request.OrderStatus);
        }

        // Tìm kiếm
        if (!string.IsNullOrEmpty(request.SearchString))
        {
            query = query.Where(x => x.PhoneNumber.Contains(request.SearchString));         
        }

        var paginatedQuery = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize + 1);

        var queryResult = await paginatedQuery.ToListAsync();

        bool hasNext = queryResult.Count > request.PageSize;

        var result = new PaginationResponse<OrderVm>
        {
            PageNumber = request.PageNumber,
            HasNext = hasNext,
            Data = queryResult.Take(request.PageSize).ToList()
        };

        return Result<PaginationResponse<OrderVm>>.Success(result);
    }



    public async Task<Result<OrderVm>> GetOrderDetailForStaff(Guid orderId)
    {
        try
        {
            Order? order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderId);

            var query = from a in _context.OrderDetails
                        join b in _context.Orders on a.OrderId equals b.Id
                        join c in _context.ProductDetails on a.ProductDetailId equals c.Id
                        join d in _context.Products on c.ProductId equals d.Id
                        join p in _context.ProductImages on c.Id equals p.ProductDetailId
                        join i in _context.Images on p.ImageId equals i.Id
                        join h in _context.Colors on c.ColorId equals h.Id
                        join k in _context.Sizes on c.SizeId equals k.Id
                        where a.OrderId == orderId
                        select new OrderDetailVm
                        {
                            Id = a.Id,
                            OrderId = b.Id,
                            ProductDetailId = c.Id,
                            ProductName = d.Name,
                            Price = a.Price,
                            Quantity = a.Quantity,
                            SizeNumber = k.SizeNumber,
                            ColorName = h.Name,
                            ImagePath = i.ImagePath,
                        };

            var userStaff = await _context.Staffs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.StaffId);
            var userCustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.CustomerId);
            var payment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == order.Id);

            OrderVm vm = new()
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                CreatedOn = order.CreatedOn,
                ConfirmedDate = order.ConfirmedDate,
                ShippedDate = order.ShippedDate,
                CompletedDate = order.CompletedDate,
                PaymentStatus = payment.Status,
                PaymentMethod = payment.PaymentMethod,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                Note = order.Note,
                StaffId = order.StaffId,
                CustomerId = order.CustomerId,
                Staff = userStaff.LastName + userStaff.FirstName,
                Customer = userCustomer.LastName + userCustomer.FirstName,
                PhoneNumber = order.PhoneNumber,
                ShipAddress = order.ShipAddress,
                ShipAddressDetail = order.ShipAddressDetail,
                Details = await query.AsNoTracking().ToListAsync(),
            };

            return Result<OrderVm>.Success(vm);
        }
        catch (Exception ex)
        {
            return Result<OrderVm>.Error(ex.Message);
        }
    }


    private async Task<Result> SendEmail(Order order, List<OrderDetail> orderDetails)
    {
        var customer = _context.Customers.FirstOrDefault(x => x.Id == order.CustomerId);
        var payment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == order.Id);
        if (customer == null)
        {
            return Result<bool>.Invalid("customer không tồn tại");
        }

        string subject = "Thông tin đơn hàng của bạn từ shop CoolSharp";

        // Đọc nội dung của file template email
        string filePath = Path.Combine("wwwroot", "content-email", "SendEmail.html");
        if (!File.Exists(filePath))
        {
            return Result<bool>.Invalid("file không tồn tại");
        }
        var body = await File.ReadAllTextAsync(filePath);

        // Thay thế nội dung động vào template
        var cultureInfo = new CultureInfo("vi-VN");
        body = body.Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                   .Replace("{OrderCode}", order.OrderCode)
                   .Replace("{PaymentMethod}",payment.PaymentMethod.ToString())
                   .Replace("{TotalPrice}", order.TotalPrice.ToString("C", cultureInfo))
                   .Replace("{ShipAddress}", order.ShipAddress)
                   .Replace("{ShipAddressDetail}", order.ShipAddressDetail)
                   .Replace("{PhoneNumber}", order.PhoneNumber)
                   .Replace("{EmailAddress}", customer.EmailAddress);

        var productRows = new StringBuilder();
        int stt = 1;
        foreach (var item in orderDetails)
        {
            var productDetail = await _context.ProductDetails.Include(pd => pd.Product)
                .FirstOrDefaultAsync(x => x.Id == item.ProductDetailId);

            if (productDetail != null)
            {
                var productName = productDetail?.Product?.Name;
                productRows.Append($@"
                <tr>
                    <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word"">
                        {stt}
                    </td>
                    <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word"">
                        {productName}
                    </td>
                    <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif"">
                        {item.Quantity}
                    </td>
                    <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif"">
                        {item.Price.ToString("C", cultureInfo)}
                    </td>
                </tr>");
                stt++;
            }
        }

        // Thêm phần HTML động vào template
        body = body.Replace("{ProductRows}", productRows.ToString());

        var message = new Message(new[] { customer.EmailAddress }, subject, body);
        await _emailService.SendEmailWithHtml(message);
        return Result.Success();
    }




    public async Task<Result<bool>> UpdateOrderStatusForStaff(UpdateOrderForStaffCommand request)
    {

        try
        {
            Order? exist = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
            string orderstatus = "";

            if (exist == null)
            {
                return Result<bool>.Invalid("Order không tồn tại");
            }
            switch (request.OrderStatus)
            {
                case OrderStatus.Pending:
                    exist.ConfirmedDate = DateTime.Now;
                    exist.OrderStatus = OrderStatus.AwaitingShipment;
                    if (exist != null)
                    {
                        List<OrderDetail> orderDetails = await _context.OrderDetails.AsNoTracking().Where(x => x.OrderId == exist.Id).ToListAsync();
                        foreach (var detail in orderDetails)
                        {
                            // Lấy sp
                            ProductDetail? productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == detail.ProductDetailId);
                            if (productDetail != null)
                            {
                                // Trừ sp theo order
                                if (productDetail.Stock >= detail.Quantity)
                                {
                                    productDetail.Stock -= detail.Quantity;
                                    _context.ProductDetails.Update(productDetail);
                                }
                            }
                        }
                        
                        await SendEmail(exist, orderDetails);
                    }

                    orderstatus = "xác nhận. Đang được đóng gói và chờ shipper nhận hàng.";
                    break;
                case OrderStatus.AwaitingShipment:
                    exist.ShippedDate = DateTime.Now;
                    exist.OrderStatus = OrderStatus.AWaitingPickup;
                    orderstatus = "vận chuyển và sẽ giao cho bạn trong thời gian tới.";

                    break;
                case OrderStatus.AWaitingPickup:
                    exist.CompletedDate = DateTime.Now;
                    exist.OrderStatus = OrderStatus.Completed;
                    orderstatus = "giao thành công.";
                    var transaction = from o in _context.Orders
                                      join p in _context.Payments on o.Id equals p.OrderId
                                      where o.Id == exist.Id
                                      select p;
                    Payment payment = await transaction.FirstOrDefaultAsync();
                    //payment.Status = "Đã thanh toán";
                    _context.Payments.Update(payment);
                    break;

            }

            _context.Orders.Update(exist);
            //var notification = new Notification
            //{
            //    UserId = exist.UserId,
            //    Message = $"Đơn hàng {exist.OrderCode} đã được {orderstatus}",
            //    CreatedOn = DateTime.Now,
            //    IsRead = false
            //};

            //await _context.Notifications.AddAsync(notification);
            //await _hubContext.Clients.User(exist.UserId.ToString()).SendAsync("SendOrderUpdate", notification.Message);
            //await _hubContext.Clients.All.SendAsync("SendOrderUpdate", notification.Message);

            return Result<bool>.Success(true/*, "Cập nhật status Order thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async  Task<Result<bool>> CancelOrderForStaff(CancelOrderForStaffCommand request, CancellationToken token)
    {
        try
        {
            var exist = await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (exist == null)
            {
                return Result<bool>.Error("Không tìm thấy đơn hàng");
            }
            var orderDetails = await _context.OrderDetails
                .Where(x => x.OrderId == exist.Id)
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                var productDetail = await _context.ProductDetails
                    .FirstOrDefaultAsync(x => x.Id == detail.ProductDetailId);

                if (productDetail != null)
                {
                    productDetail.Stock += detail.Quantity;
                    _context.ProductDetails.Update(productDetail);
                }
            }
            //huy orderdetail
            exist.OrderStatus = OrderStatus.Cancelled;
            _context.Orders.Update(exist);

            return Result<bool>.Success(true/*, "Hủy Order thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<List<OrderDetailVm>>> TopProducts()
    {
        try
        {
            var topProducts = await (from a in _context.OrderDetails
                                     join b in _context.Orders on a.OrderId equals b.Id
                                     join c in _context.ProductDetails on a.ProductDetailId equals c.Id
                                     join d in _context.Products on c.ProductId equals d.Id
                                     join p in _context.ProductImages on c.Id equals p.ProductDetailId
                                     join i in _context.Images on p.ImageId equals i.Id
                                     join h in _context.Colors on c.ColorId equals h.Id
                                     join k in _context.Sizes on c.SizeId equals k.Id
                                     where b.OrderStatus == OrderStatus.Completed 
                                     group new { a, b, c, d, p, i, h, k } by a.ProductDetailId into g
                                     select new
                                     {
                                         Id = g.Select(x => x.a.Id).FirstOrDefault(),
                                         OrderId = g.Select(x => x.b.Id).FirstOrDefault(),
                                         ProductDetailId = g.Key,
                                         TotalQuantity = g.Sum(x => x.a.Quantity),
                                         SizeNumber = g.Select(x => x.k.SizeNumber).FirstOrDefault(),
                                         ColorName = g.Select(x => x.h.Name).FirstOrDefault(),
                                         ImagePath = g.Select(x => x.i.ImagePath).FirstOrDefault(),
                                         ProductName = g.Select(x => x.d.Name).FirstOrDefault(),
                                         PaidPrice = g.Select(x => x.a.Price).FirstOrDefault(),
                                     })
                                  .OrderByDescending(x => x.TotalQuantity)
                                  .Take(10) 
                                  .Select(x => new OrderDetailVm
                                  {
                                      Id = x.Id,
                                      OrderId = x.OrderId,
                                      ProductDetailId = x.ProductDetailId,
                                      ProductName = x.ProductName,
                                      Price = x.PaidPrice,
                                      Quantity = x.TotalQuantity,
                                      SizeNumber = x.SizeNumber,
                                      ColorName = x.ColorName,
                                      ImagePath = x.ImagePath,
                                      
                                  }).ToListAsync();

            return Result<List<OrderDetailVm>>.Success(topProducts);
        }
        catch (Exception ex)
        {
            return Result<List<OrderDetailVm>>.Error(ex.Message);
        }
    }

    public async Task<Result<List<OrderVm>>> Statistical()
    {
        try
        {
            var orders = await (from a in _context.Orders
                                join b in _context.OrderDetails on a.Id equals b.OrderId into orderGroup
                                select new OrderVm
                                {
                                    Id = a.Id,
                                    OrderCode = a.OrderCode,
                                    CreatedOn = a.CreatedOn,
                                    ConfirmedDate = a.ConfirmedDate,
                                    ShippedDate = a.ShippedDate,
                                    CompletedDate = a.CompletedDate,
                                    TotalPrice = orderGroup.Sum(d => d.Price * d.Quantity),
                                    OrderStatus = a.OrderStatus,
                                    Note = a.Note,
                                    StaffId = a.StaffId,
                                    CustomerId = a.CustomerId,
                                    PhoneNumber = a.PhoneNumber,
                                    ShipAddress = a.ShipAddress,
                                    ShipAddressDetail = a.ShipAddressDetail,

                                }).ToListAsync();



            return Result<List<OrderVm>>.Success(orders);
        }
        catch (Exception ex)
        {
            return Result<List<OrderVm>>.Error(ex.Message);
        }
    }




    #endregion
}
