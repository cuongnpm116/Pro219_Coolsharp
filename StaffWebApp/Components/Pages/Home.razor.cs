using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Enum;

namespace StaffWebApp.Components.Pages;

public partial class Home
{
    [Inject]
    private IOrderService OrderService { get; set; }
    private List<OrderVm> _Statistical = new();
    private List<OrderDetailVm> _lstOrderDetail = new();
    private List<ProductDetailInOrderVm> _lstProductDetail = new();
    private OrderPaginationRequest _paginationRequest = new();
    private string _imageUrl = ShopConstants.EShopApiHost + "/product-content/";

    protected override async Task OnInitializedAsync()
    {
        await Statistical();
        await TopProducts();
        await LowStockProducts();
        await BarChart();
        await PieChart();
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

    private DateTime? BeginDate;
    private DateTime? EndDate;
    private string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    private List<ChartSeries> Series = new List<ChartSeries>();
    private double[] data;
    private string[] labels;
    private async Task BeginOnchand(DateTime? newBegin)
    {
        BeginDate = newBegin;
        await BarChart();
        await PieChart();
        StateHasChanged();
    }

    private async Task EndOnchand(DateTime? newBegin)
    {
        EndDate = newBegin;
        await BarChart();
        await PieChart();
        StateHasChanged();
    }

    private async Task BarChart()
    {
        var response = await OrderService.Statistical();
        if (response != null && response.Value != null)
        {
            var orderList = response.Value;
            if (BeginDate.HasValue && EndDate.HasValue)
            {
                orderList = orderList.Where(o => o.CompletedDate >= BeginDate.Value && o.CompletedDate <= EndDate.Value).ToList();
            }
            var monthlyRevenue = orderList
                .Where(order => order.OrderStatus == OrderStatus.Completed && order.CompletedDate.HasValue)
                .GroupBy(order => new { order.CompletedDate.Value.Year, order.CompletedDate.Value.Month })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    TotalRevenue = group.Sum(order => order.TotalPrice)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();
            var revenueDataByYear = new Dictionary<int, double[]>();

            foreach (var item in monthlyRevenue)
            {
                if (!revenueDataByYear.ContainsKey(item.Year))
                {
                    revenueDataByYear[item.Year] = new double[12];
                }
                revenueDataByYear[item.Year][item.Month - 1] = (double)item.TotalRevenue;
            }

            Series = revenueDataByYear.Select(kvp => new ChartSeries
            {
                Name = $"Doanh thu Năm {kvp.Key}",
                Data = kvp.Value
            }).ToList();
        }
    }


    private async Task PieChart()
    {
        var response = await OrderService.Statistical();
        if (response.Value != null)
        {
            var orderList = response.Value;
            if (BeginDate.HasValue && EndDate.HasValue)
            {
                orderList = orderList.Where(o => o.CompletedDate >= BeginDate.Value && o.CompletedDate <= EndDate.Value).ToList();
            }

            var statusCounts = orderList
                .GroupBy(o => o.OrderStatus)
                .Select(g => new { Status = EnumUtility.ConvertOrderStatus(g.Key), Count = g.Count() })
                .ToList();

            var totalOrders = statusCounts.Sum(sc => sc.Count);
            labels = statusCounts
                .Select(sc => $"{sc.Status} ({(sc.Count * 100.0 / totalOrders):0.##}%)") // Thêm phần trăm vào nhãn
                .ToArray();

            data = statusCounts
                .Select(sc => (double)sc.Count)
                .ToArray();
        }
    }

    #endregion
}
