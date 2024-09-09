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
    private List<ProductDetailVm> _lstProductDetail = new();
    private OrderPaginationRequest _paginationRequest = new();
    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";

    protected override async Task OnInitializedAsync()
    {
        await Statistical();
        await TopProducts();
        await LowStockProducts();
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









    private int Index = -1; //default value cannot be 0 -> first selectedindex is 0.

    public List<OrderVm> Series = new List<OrderVm>()
    {

    };
    public string[] XAxisLabels = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };




    private async Task barchar()
    {
        var response = await OrderService.Statistical();

        if (response.Value != null)
        {
            Series = response.Value;
            StateHasChanged();
        }
    }




















        int dataSize = 4;
    double[] data = { 77, 25, 20, 5 };
    string[] labels = { "Uranium", "Plutonium", "Thorium", "Caesium", "Technetium", "Promethium",
                        "Polonium", "Astatine", "Radon", "Francium", "Radium", "Actinium", "Protactinium",
                        "Neptunium", "Americium", "Curium", "Berkelium", "Californium", "Einsteinium", "Mudblaznium" };

    Random random = new Random();

    void RandomizeData()
    {
        var new_data = new double[dataSize];
        for (int i = 0; i < new_data.Length; i++)
            new_data[i] = random.NextDouble() * 100;
        data = new_data;
        StateHasChanged();
    }

    void AddDataSize()
    {
        if (dataSize < 20)
        {
            dataSize = dataSize + 1;
            RandomizeData();
        }
    }
    void RemoveDataSize()
    {
        if (dataSize > 0)
        {
            dataSize = dataSize - 1;
            RandomizeData();
        }
    }



}
