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
    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    protected override async Task OnInitializedAsync()
    {
        await Statistical();
        await TopProducts();
        //await LowStockProducts();
        BarChart();
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

    private async Task TopProducts()
    {
        var response = await OrderService.TopProducts();

        if (response.Value != null)
        {
            _lstOrderDetail = response.Value;
            StateHasChanged();
        }
    }
    private async Task OnStockChanged(int newStock)
    {
        _paginationRequest.Stock = newStock;
        //await LowStockProducts();
        StateHasChanged();
    }

    //private async Task LowStockProducts()
    //{
    //    var response = await OrderService.LowStockProducts(_paginationRequest);

    //    if (response.Value != null)
    //    {
    //        _lstProductDetail = response.Value;
    //        await TimeBasedData();
    //        StateHasChanged();
    //    }
    //}



    public string[] XAxisLabels = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
            "Tháng 9","Tháng 10","Tháng 11","Tháng 12" };
    public List<ChartSeries> Series { get; set; } = new();

    private void BarChart()
    {
        var monthlyRevenue = _Statistical
            .Where(order => order.OrderStatus == OrderStatus.Completed && order.CompletedDate.HasValue) // lấy hóa đơn đã hoàn thành
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
            Name = $"Danh thu Năm {kvp.Key}",
            Data = kvp.Value
        }).ToList();
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



}
