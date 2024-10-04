using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Enum;
using WebAppIntegrated.SignalR;

namespace StaffWebApp.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private IOrderService OrderService { get; set; }
        [CascadingParameter]
        private SignalRService SignalRService { get; set; }
        [Inject]
        private ISnackbar Snackbar { get; set; }

        private List<OrderVm> _Statistical = new();
        private List<OrderDetailVm> _lstOrderDetail = new();
        private List<ProductDetailInOrderVm> _lstProductDetail = new();
        private OrderPaginationRequest _paginationRequest = new();
        private string _imageUrl = ShopConstants.EShopApiHost + "/product-content/";

        private DateTime? BeginDate;
        private DateTime? EndDate;
        private double[] data;
        private string[] labels;

        protected override async Task OnInitializedAsync()
        {

            await Statistical();
            await TopProducts();
            await LowStockProducts();
            await PieChart();
            if (SignalRService == null || SignalRService._hubConnection == null)
            {
                throw new ArgumentNullException(nameof(SignalRService), "SignalRService is null or HubConnection is not initialized.");
            }
            SignalRService._hubConnection.On<string>("UpdateDatabase", async (message) =>
            {
                await Statistical();
                await TopProducts();
                await LowStockProducts();
                await PieChart();
                InvokeAsync(StateHasChanged);
            });
            StateHasChanged();
        }

        private async Task Statistical()
        {
            var response = await OrderService.Statistical();
            if (response.Value != null)
            {
                _Statistical = response.Value;
                StateHasChanged();
            }
        }

        #region Products

        private async Task TopProducts()
        {
            var response = await OrderService.TopProducts(_paginationRequest);
            if (response.Value != null)
            {
                _lstOrderDetail = response.Value;
                StateHasChanged();
            }
        }

        private async Task LowStockProducts()
        {
            var response = await OrderService.LowStockProducts();
            if (response.Value != null)
            {
                _lstProductDetail = response.Value;
                StateHasChanged();
            }
        }

        private async Task OnStockChanged(int newStock)
        {
            _paginationRequest.Stock = newStock;
            await TopProducts();
            StateHasChanged();
        }

        private async Task OnBeginDateChanged(DateTime? newBegin)
        {
            _paginationRequest.Begin = newBegin;
            await TopProducts();
            StateHasChanged();
        }

        private async Task OnEndDateChanged(DateTime? newEnd)
        {
            _paginationRequest.End = newEnd;
            await TopProducts();
            StateHasChanged();
        }

        #endregion

        #region BarChart

        private async Task BeginOnchand(DateTime? newBegin)
        {
            BeginDate = newBegin;
            await UpdateChartData();
            await PieChart();
            StateHasChanged();
        }

        private async Task EndOnchand(DateTime? newEnd)
        {
            EndDate = newEnd;
            await UpdateChartData();
            await PieChart();
            StateHasChanged();
        }

        private async Task UpdateChartData()
        {
            var response = await OrderService.Statistical();
            var orderList = response.Value;

            if (BeginDate.HasValue && EndDate.HasValue)
            {
                var newEnd = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                orderList = orderList.Where(o => o.CompletedDate >= BeginDate.Value && o.CompletedDate <= newEnd).ToList();
            }

            // Lọc đơn hàng chỉ với OrderStatus.Completed
            var completedOrders = orderList
                .Where(order => order.OrderStatus == OrderStatus.Completed)
                .ToList();

            // Tính tổng doanh thu theo tháng
            var monthlyRevenue = new decimal[12];

            foreach (var order in completedOrders)
            {
                var month = order.CompletedDate.Value.Month - 1; // Tháng 0-11
                monthlyRevenue[month] += order.TotalPrice;
            }

            var chartData = new
            {
                labels = new[]
                {
                    "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5",
                    "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10",
                    "Tháng 11", "Tháng 12"
                },
                data = monthlyRevenue.Select(mr => (double)mr).ToArray() // Chuyển đổi decimal sang double nếu cần
            };

            await JS.InvokeVoidAsync("updateChart", chartData);
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Gọi UpdateChartData để cập nhật dữ liệu ban đầu khi component lần đầu được render
                await UpdateChartData();
            }
        }

        #endregion

        #region PieChart

        private async Task PieChart()
        {
            var response = await OrderService.Statistical();
            if (response.Value != null)
            {
                var orderList = response.Value;

                if (BeginDate.HasValue && EndDate.HasValue)
                {
                    var newEnd = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                    orderList = orderList.Where(o => o.CreatedOn >= BeginDate.Value && o.CreatedOn <= newEnd).ToList();
                }

                var statusCounts = orderList
                    .GroupBy(o => o.OrderStatus)
                    .Select(g => new { Status = EnumUtility.ConvertOrderStatus(g.Key), Count = g.Count() })
                    .ToList();

                var totalOrders = statusCounts.Sum(sc => sc.Count);
                labels = statusCounts
                    .Select(sc => $"{sc.Status} ({sc.Count} đơn, {(sc.Count * 100.0 / totalOrders):0.##}%)") // Thêm số đơn hàng và phần trăm vào nhãn
                    .ToArray();

                data = statusCounts
                    .Select(sc => (double)sc.Count)
                    .ToArray();
            }
        }

        #endregion
    }
}
