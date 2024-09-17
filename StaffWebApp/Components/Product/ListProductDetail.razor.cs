using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Product;
public partial class ListProductDetail
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IProductService ProductService { get; set; } = null!;

    [Inject]
    private IColorService ColorService { get; set; } = null!;

    [Inject]
    private ISizeService SizeService { get; set; } = null!;

    [CascadingParameter]
    public MudDialogInstance DialogInstance { get; set; } = null!;

    [Parameter]
    public Guid ProductId { get; set; }

    private IEnumerable<ColorForSelectVm> _colors = [];
    private IEnumerable<SizeForSelectVm> _sizes = [];
    private IEnumerable<DetailVm> _details = [];

    private DetailVm _selectedDetail;
    private DetailVm _backupDetail;
    private DetailVmValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
        await GetColor();
        await GetSize();
        await GetDetails();
    }

    private async Task GetDetails()
    {
        _details = await ProductService.GetDetails(ProductId);
    }

    private async Task GetColor()
    {
        var colors = await ColorService.GetColorForSelectVms();
        _colors = colors;
    }

    private async Task GetSize()
    {
        var sizes = await SizeService.GetSizeForSelectVms();
        _sizes = sizes;
    }

    private static string GetClassification(string colorName, int sizeNumber)
    {
        return colorName + ", " + sizeNumber;
    }

    private async Task UpdateDetail(object detail)
    {
        if (detail is DetailVm detailVm)
        {
            bool result = await ProductService.UpdateDetailAsync(detailVm);
            if (result)
            {
                _selectedDetail = null;
                await GetDetails();
            }
            bool exist = _details.Any(x => x.Id == detailVm.Color.Id);

        }
        else
        {
            Snackbar.Add("Chi tiết sản phẩm không hợp lệ", Severity.Error);
            return;
        }
    }

    private void EditDetail(object detail)
    {
        _selectedDetail = detail as DetailVm;
        _backupDetail = new DetailVm
        {
            Id = _selectedDetail.Id,
            Size = _selectedDetail.Size,
            Color = _selectedDetail.Color,
            OriginalPrice = _selectedDetail.OriginalPrice,
            Price = _selectedDetail.Price,
            Stock = _selectedDetail.Stock
        };
    }

    private void SetToOriginalValue(object item)
    {
        var detail = item as DetailVm;
        detail.Size = _backupDetail.Size;
        detail.Color = _backupDetail.Color;
        detail.OriginalPrice = _backupDetail.OriginalPrice;
        detail.Price = _backupDetail.Price;
        detail.Stock = _backupDetail.Stock;
    }

    private void ChangeColor(ColorForSelectVm color)
    {
        _selectedDetail.Color = color;
    }

    private void OnStockLoseFocus()
    {
        if (_selectedDetail.Stock < 0)
        {
            _selectedDetail.Stock = 0;
        }
    }

    private async Task<IEnumerable<ColorForSelectVm>> SearchColors(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return await Task.FromResult(_colors);
        }
        return await Task.FromResult(_colors.Where(x => x.Name.Contains(value, StringComparison.CurrentCultureIgnoreCase)));
    }

    private async Task<IEnumerable<SizeForSelectVm>> SearchSizes(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return await Task.FromResult(_sizes);
        }
        return await Task.FromResult(_sizes.Where(x => x.SizeNumber.ToString().Contains(value)));
    }
}