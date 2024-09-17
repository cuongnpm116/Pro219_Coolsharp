using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    [Parameter]
    public string SearchString { get; set; } = "";
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; } = null;
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
    public Guid _staffId;
    [Parameter] public EventCallback<OrderStatus> OnOrderStatusChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        AuthenticationState? authState = await AuthStateTask;
        _staffId = new(authState.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);

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
    void OrderDetail(Guid OrderId)
    {
        NavigationManager.NavigateTo($"/OrderDetail/{OrderId}");
    }

    #region Search

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
    #endregion

    #region Page
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
    #endregion

    #region Loc
    private async Task StatusChanged(OrderStatus newStatus)
    {
        _paginationRequest.OrderStatus = newStatus;
        await LoadOrder();
        StateHasChanged();
    }
    #endregion

    #region UpdateOrder
    private async Task UpdateOrder(Guid orderId, OrderStatus OrderStatus)
    {
        bool? result = await DialogService.ShowMessageBox("Cảnh báo",
                                                          "Bạn có chắc chắn muốn thay đổi trạng thái đơn hàng?",
                                                          yesText: "Thay đổi",
                                                          cancelText: "Hủy");
        if (result == true)
        {
            var nextStatus = GetNextStatus(OrderStatus);
            var updateRequest = new OrderVm
            {
                Id = orderId,
                OrderStatus = nextStatus,
                StaffId= _staffId,
                
            };
            try
            {
                await OrderService.UpdateOrderStatus(updateRequest);
                Snackbar.Add("Thay đổi trạng thái đơn hàng thành công", Severity.Success);
                // Gọi callback để thông báo trạng thái mới
                await OnOrderStatusChanged.InvokeAsync(nextStatus);

                await LoadOrder();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Thay đổi trạng thái đơn hàng thất bại. Vui lòng thử lại !", Severity.Error);
            }
            finally
            {
                MudDialog?.Close(DialogResult.Cancel());
            }
        }
    }

    protected async Task CancelOrder(Guid orderId)
    {
        CancelOrderRequest cancelOrder = new();
        cancelOrder.OrderId = orderId;
        cancelOrder.ModifiedBy = _staffId;
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

    private OrderStatus GetNextStatus(OrderStatus OrderStatus)
    {
        return OrderStatus switch
        {
            OrderStatus.Pending => OrderStatus.AwaitingShipment,
            OrderStatus.AwaitingShipment => OrderStatus.AWaitingPickup,
            OrderStatus.AWaitingPickup => OrderStatus.Completed,
        };
    }

    #endregion

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
        catch (Exception ex)
        {
            Snackbar.Add("Xuất ra excel thất bại", Severity.Error);
        }
    }

    

}

