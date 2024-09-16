using CustomerWebApp.Service.Order;
using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Components.Customer;

public partial class PurchaseOrder
{
    [Parameter]
    public Guid CustomerId { get; set; }
    [Inject]
    private IOrderService OrderService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private string _imageUrl = ShopConstants.EShopApiHost + "/product-content/";

    private GetOrdersWithPaginationRequest _request = new()
    {
        PageSize = 5
    };
    private PaginationResponse<OrderVm> _paginatedOrders = new();
    private IReadOnlyList<OrderDetailVm> _orderDetail;
   
    protected override async Task OnInitializedAsync()
    {
        _request.CustomerId = CustomerId;
        await GetOrders();
    }
    private Guid? _expandedOrderId; 
    private async Task OnOrderRowClick(TableRowClickEventArgs<OrderVm> args)
    {
        if (_expandedOrderId == args.Item.Id)
        {
            _expandedOrderId = null; 
            _orderDetail = null; 
        }
        else
        {
            var result = await OrderService.GetOrderDetail(args.Item.Id);
            if (result.Value != null)
            {
                _orderDetail = result.Value;
                _expandedOrderId = args.Item.Id; 
            }
        }
    }

    private async Task GetOrders()
    {
        var result = await OrderService.GetOrdersForCustomerWithPagination(_request);
        _paginatedOrders = result.Value;
    }

    private async Task SearchClick()
    {
        _request.PageNumber = 1;
        await GetOrders();
    }

    private async Task CancelOrder(Guid orderId, string orderCode)
    {
        CancelOrderRequest request = new();
        request.OrderId = orderId;
        request.ModifiedBy = CustomerId;
        var result = await OrderService.CancelOrder(request);
        if (result is not null && result.Value)
        {
            Snackbar.Add($"Hủy đơn hàng {orderCode} thành công", Severity.Success);
            await GetOrders();
        }
        else
        {
            Snackbar.Add($"Hủy đơn hàng {orderCode} thất bại", Severity.Error);
        }
    }

    private async Task OnNextPageClicked()
    {
        if (_paginatedOrders.HasNext)
        {
            _request.PageNumber++;
            await GetOrders();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        _request.PageNumber--;
        await GetOrders();
    }
    private async Task OnSelectedTabChanged(int index)
    {
        _request.OrderStatus = index switch
        {
            0 => OrderStatus.None,
            1 => OrderStatus.Pending,
            2 => OrderStatus.AwaitingShipment,
            3 => OrderStatus.Completed,
            4 => OrderStatus.Cancelled,
            _ => OrderStatus.None
        };
        await GetOrders();
    }
}