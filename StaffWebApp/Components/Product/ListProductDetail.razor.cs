using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Color;
using StaffWebApp.Components.Product.Update;
using StaffWebApp.Components.Size;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Product.Dtos;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Product;
public partial class ListProductDetail
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

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

    //[Parameter]
    //public Action OnSuccess { get; set; }

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
        try
        {
            if (detail is DetailVm detailVm)
            {
                if (IsDetailModified(detailVm))
                {
                    var existDetailId = await ProductService.CheckUpdateExistDetail(ProductId, detailVm.Color.Id, detailVm.Size.Id);
                    if (await HandleExistingDetail(detailVm, existDetailId))
                    {
                        return;
                    }

                    if (!await ProductService.CheckColorExistedInProduct(ProductId, detailVm.Color.Id))
                    {
                        if (await HandleNewColor(detailVm))
                        {
                            return;
                        }
                    }
                }

                await FinalizeUpdate(detailVm);
            }
            else
            {
                Snackbar.Add("Chi tiết sản phẩm không hợp lệ", Severity.Error);
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            //OnSuccess.Invoke();
        }
    }

    private bool IsDetailModified(DetailVm detailVm)
    {
        return detailVm.Color.Id != _backupDetail.Color.Id || detailVm.Size.Id != _backupDetail.Size.Id;
    }

    private async Task<bool> HandleExistingDetail(DetailVm detailVm, Guid existDetailId)
    {
        if (existDetailId != Guid.Empty)
        {
            await DialogService.ShowMessageBox(
                "Thông báo",
                $"Biến thể {GetClassification(detailVm.Color.Name, detailVm.Size.SizeNumber)} đã tồn tại. Vui lòng chọn lại biến thể.",
                yesText: "Xác nhận");

            _selectedDetail = null;
            _backupDetail = null;
            await GetDetails();
            StateHasChanged();
            return true;
        }
        return false;
    }

    private async Task<bool> HandleNewColor(DetailVm detailVm)
    {
        await DialogService.ShowMessageBox("Thông báo", "Hiện tại chưa có ảnh cho màu này. Vui lòng thêm ảnh mới", yesText: "Xác nhận");
        var dialog = await DialogService.ShowAsync<UploadNewImageForNewColor>();
        var dialogResult = await dialog.Result;

        if (dialogResult.Data is List<ImageDto> newImages && await ProductService.UpdateDetailWithNewImages(detailVm, newImages))
        {
            Snackbar.Add("Cập nhật chi tiết thành công", Severity.Success);
            _selectedDetail = null;
            await GetDetails();
            StateHasChanged();
            return true;
        }
        else
        {
            Snackbar.Add("Cập nhật chi tiết thất bại", Severity.Error);
            return false;
        }
    }

    private async Task FinalizeUpdate(DetailVm detailVm)
    {
        bool result = await ProductService.UpdateDetailAsync(detailVm);
        if (result)
        {
            Snackbar.Add("Cập nhật chi tiết thành công", Severity.Success);
            _selectedDetail = null;
            await GetDetails();
            StateHasChanged();
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

    private async Task OpenAddColorDialogClick()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateColor>("Thêm màu", options);
        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetColor();
        }
    }

    private async Task OpenAddSizeDialogClick()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateSize>("Thêm kích cỡ", options);
        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetSize();
        }
    }

    private async Task OpenAddDetailDiaLog()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Large, FullWidth = true };
        DialogParameters parameters = new()
        {
            { "Colors", _colors },
            { "Sizes", _sizes },
            { "ProductId", ProductId }
        };
        var dialog = await DialogService.ShowAsync<AddOneDetailForm>(
            "Thêm chi tiết sản phẩm",
            parameters,
            options);

        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetDetails();
            StateHasChanged();
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