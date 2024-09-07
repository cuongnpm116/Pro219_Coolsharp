using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Color;
using StaffWebApp.Components.Size;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product.Vms;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Product;
public partial class CreateProduct
{
    #region Inject
    [Inject]
    private IColorService ColorService { get; set; } = null!;

    [Inject]
    private ISizeService SizeService { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    #endregion

    #region Components
    private List<CreateProductDetailForm> _productDetailForms = [];
    private Dictionary<ColorForSelectVm, CreateImageForm> _imageDict = [];
    #endregion

    #region Vm
    private CreateProductInfoVm _productInfoVm = new();
    private List<CreateProductDetailVm> _productDetails = [];
    private Dictionary<Guid, List<CreateImageVm>> _imagesByColor = [];
    #endregion

    #region Field
    private IEnumerable<ColorForSelectVm> _availableColors;
    private IEnumerable<SizeForSelectVm> _availableSizes;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await GetColors();
        await GetSizes();
    }

    private void CheckData()
    {
        var x = _productInfoVm;
        var y = _productDetails;
        var z = _imagesByColor;
    }

    private void AddDetailClick()
    {
        _productDetails.Add(new CreateProductDetailVm());
        _productDetailForms.Add(new CreateProductDetailForm());
    }

    private void RemoveProductDetail(CreateProductDetailVm detail)
    {
        int index = _productDetails.IndexOf(detail);
        _productDetails.RemoveAt(index);
        _productDetailForms.RemoveAt(index);
        bool isUsedColor = _productDetails.Any(x => x.Color == detail.Color);
        if (!isUsedColor)
        {
            _imagesByColor.Remove(detail.Color.Id);
            _imageDict.Remove(detail.Color);
        }
        StateHasChanged();
    }

    private void HanldeColorSelected(ColorForSelectVm color)
    {
        if (_imagesByColor.ContainsKey(color.Id))
        {
            _imagesByColor[color.Id] = [];
            _imageDict.Add(color, new CreateImageForm());
        }
    }

    private async Task OpenAddSizeDialog()
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small
        };

        var dialog = await DialogService.ShowAsync<CreateSize>(string.Empty, dialogOptions);
        var dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await GetSizes();
        }
    }

    private async Task OpenAddColorDialog()
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small
        };

        var dialog = await DialogService.ShowAsync<CreateColor>(string.Empty, dialogOptions);
        var dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await GetColors();
        }
    }

    private async Task GetColors()
    {
        _availableColors = await ColorService.GetColorForSelectVms();
    }

    private async Task GetSizes()
    {
        _availableSizes = await SizeService.GetSizeForSelectVms();
    }
}