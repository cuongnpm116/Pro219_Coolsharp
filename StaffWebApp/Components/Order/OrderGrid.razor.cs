using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Order;

public partial class OrderGrid
{
    [Inject]
    private IOrderService OrderService { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    private PaginationResponse<OrderVm> _lstOrder = new PaginationResponse<OrderVm>();
    private OrderPaginationRequest _paginationRequest = new();
    [Parameter]
    public OrderStatus OrderStatus { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _paginationRequest.OrderStatus = OrderStatus;
        await LoadOrder();
        StateHasChanged();
    }
    private async Task LoadOrder()
    {
        var response = await OrderService.GetOrders(_paginationRequest);

        if (response.Value != null)
        {
            _lstOrder = response.Value;
            _lstOrder.Data = response.Value.Data;
        }
    }
    protected async override Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(_paginationRequest.SearchString))
        {
            _paginationRequest.PageNumber = 1;
            await LoadOrder();
            StateHasChanged();
        }

    }

    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleSearchValueChanged();
        }
    }
    private async Task HandleSearchValueChanged()
    {
        if (!string.IsNullOrEmpty(_paginationRequest.SearchString))
        {
            await LoadOrder();
            StateHasChanged();
        }
        else
        {
            _paginationRequest.PageNumber = 1;
            await LoadOrder();
            StateHasChanged();
        }
    }

    private async Task OnNextPageClicked()
    {
        if (_lstOrder.HasNext)
        {
            _paginationRequest.PageNumber++;
            await LoadOrder();
            StateHasChanged();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        if (_lstOrder.PageNumber > 0)
        {

            _paginationRequest.PageNumber--;
            await LoadOrder();
            StateHasChanged();
        }
    }
    private async Task OnStatusChanged(OrderStatus newStatus)
    {
        _paginationRequest.OrderStatus = newStatus;
        await LoadOrder();
        StateHasChanged();
    }

    private async Task UpdateOrder(Guid orderId, OrderStatus OrderStatus)
    {
        bool? messageResult = await DialogService.ShowMessageBox("Cảnh báo",
                                                   "Bạn chắc chắn với thay đổi trạng thái đơn hàng không?",
                                                   yesText: "Thay đổi",
                                                   cancelText: "Hủy");
        if (messageResult == true)
        {
            var updateStatus = new OrderVm
            {
                Id = orderId,
                OrderStatus = OrderStatus,
            };
            await OrderService.UpdateOrderStatus(updateStatus);
            Snackbar.Add("Thay đổi trạng thái đơn hàng  thành công", Severity.Success);
            await LoadOrder();
            StateHasChanged();
        }
        MudDialog?.Close(DialogResult.Cancel());
    }

    protected async Task CancelOrder(Guid orderId)
    {
        CancelOrderRequest cancelOrder = new();
        cancelOrder.OrderId = orderId;
        cancelOrder.ModifiedBy = Guid.Empty;
        bool? messageResult = await DialogService.ShowMessageBox("Cảnh báo",
                                               "Bạn chắc chắn hủy đơn hàng không?",
                                               yesText: "Xóa",
                                               cancelText: "Hủy");
        if (messageResult == true)
        {
            await OrderService.CancelOrderStatus(cancelOrder);
            Snackbar.Add("Hủy đơn thành công", Severity.Success);
            await LoadOrder();
            StateHasChanged();
        }

        MudDialog?.Close(DialogResult.Cancel());
    }
    void OrderDetail(Guid OrderId)
    {
        NavigationManager.NavigateTo($"/OrderDetail/{OrderId}");
    }

    private async Task ExportOrders()
    {
        // Code xuất dữ liệu
        try
        {
            bool success = await OrderService.ExportOrdersToExcel(_paginationRequest);
            if (success == true)
            {
                Snackbar.Add("Xuất ra excel thành công", Severity.Success);

            }
            else
            {
                Snackbar.Add("Xuất ra excel thất bại", Severity.Error);
            }
        }
        catch (Exception )
        {
            Snackbar.Add("Xuất ra excel thất bại", Severity.Error);
        }
    }
}
