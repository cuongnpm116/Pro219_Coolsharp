using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Category;
using StaffWebApp.Components.Color;
using StaffWebApp.Components.Size;
using StaffWebApp.Services.Category;
using StaffWebApp.Services.Category.ViewModel;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Product.Vms.Create;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Product.Create;
public partial class CreateProduct
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;

    [Inject]
    private IColorService ColorService { get; set; } = null!;

    [Inject]
    private ISizeService SizeService { get; set; } = null!;

    [Inject]
    private IProductService ProductService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private CreateProductInfoForm _productInfoForm = new();

    // phải dùng dictionary vì
    // 1. không thể dùng list -> foreach -> không thể @ref (phải immutable)
    // 2. vòng for với i++ hay ++i đều sẽ bị out of range (nó sẽ render lần i++ cuối cùng và list đấy làm gì có i++ cuối cùng)
    // ví dụ cho số 2: list có count = 3 với vòng for i = 0; i < 3; i++. tới lần lặp thứ 4 i = 3 -> phải dừng lại
    // nhưng blazor vẫn tiếp tục render với i = 3 mà i = 3 không tồn tại -> lỗi
    private Dictionary<ColorForSelectVm, CreateImagesByColorForm> _imagesByColorForms = [];
    private Dictionary<ProductDetailVm, CreateProductDetailForm> _productDetailForms = [];

    private IEnumerable<ColorForSelectVm> _availableColors;
    private IEnumerable<SizeForSelectVm> _availableSizes;
    private IEnumerable<CategoryVm> _availableCategories = [];

    protected override async Task OnInitializedAsync()
    {
        await GetCategories();
        await GetAvailableColors();
        await GetAvailableSizes();
    }

    private async Task AddProduct()
    {
        if (_productDetailForms.Count == 0)
        {
            Snackbar.Add("Hãy thêm ít nhất 1 chi tiết cho sản phẩm", Severity.Warning);
        }
        if (_imagesByColorForms.Count == 0)
        {
            Snackbar.Add("Hãy chọn màu cho chi tiết sản phẩm và thêm ít nhất 1 ảnh cho màu đó", Severity.Warning);
        }
        bool isValidProductInfo = await _productInfoForm.ValidateAsync();
        List<Task<bool>> validateProductDetailTasks = _productDetailForms.Values.Select(x => x.ValidateAsync()).ToList();
        bool[] isValidProductDetails = await Task.WhenAll(validateProductDetailTasks);
        List<bool> isValidImages = _imagesByColorForms.Values.Select(x => x.Validate()).ToList();
        if (isValidProductDetails.Any(check => !check) || !isValidProductInfo || isValidImages.Any(check => !check))
        {
            Snackbar.Add("Vui lòng kiểm tra lại thông tin bạn nhập", Severity.Error);
            return;
        }
        bool? confirm = await DialogService.ShowMessageBox(
            "Xác nhận",
            $"Bạn chắc chắn muốn thêm sản phẩm {_productInfoForm._product.Name}",
            yesText: "Có",
            cancelText: "Hủy");

        var result = await ProductService.CreateProductAsync(
            _productInfoForm._product,
            [.. _productDetailForms.Keys],
            _imagesByColorForms.ToDictionary(x => x.Key.Id, x => x.Value.Images)
        );
        if (result)
        {
            Snackbar.Add("Thêm sản phẩm thành công", Severity.Success);
        }
        else
        {
            Snackbar.Add("Thêm sản phẩm thất bại", Severity.Error);
        }
    }

    private void AddProductDetail()
    {
        _productDetailForms.Add(new ProductDetailVm(), new CreateProductDetailForm());
    }

    private void RemoveProductDetail(ProductDetailVm productDetail)
    {
        _productDetailForms.Remove(productDetail);
        if (productDetail.Color is null || productDetail.Color.Id == Guid.Empty)
        {
            return;
        }
        bool isUsedColor = _productDetailForms.Values.Any(x => x.ProductDetail.Color == productDetail.Color);
        if (!isUsedColor)
        {
            _imagesByColorForms.Remove(productDetail.Color);
        }
    }

    private void HandleColorSelectedEvent(ColorForSelectVm color)
    {
        if (!_imagesByColorForms.ContainsKey(color))
        {
            _imagesByColorForms.Add(color, new CreateImagesByColorForm());
        }

        List<ColorForSelectVm> toRemove = [];
        foreach (var item in _imagesByColorForms.Keys)
        {
            if (!_productDetailForms.Keys.Any(x => x.Color == color))
            {
                toRemove.Add(item);
            }
        }
        foreach (var item in toRemove)
        {
            _imagesByColorForms.Remove(item);
        }
    }

    private async Task OpenAddCategoryDialogClick()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateCategoryDialog>("Thêm danh mục", options);
        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetCategories();
        }
    }

    private async Task OpenAddColorDialogClick()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateColor>("Thêm màu", options);
        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetAvailableColors();
        }
    }

    private async Task OpenAddSizeDialogClick()
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateSize>("Thêm kích cỡ", options);
        var dialogResult = await dialog.Result;
        if (dialogResult is not null && !dialogResult.Canceled)
        {
            await GetAvailableSizes();
        }
    }

    private async Task GetAvailableColors()
    {
        _availableColors = await ColorService.GetColorForSelectVms();
    }

    private async Task GetAvailableSizes()
    {
        _availableSizes = await SizeService.GetSizeForSelectVms();
    }

    private async Task GetCategories()
    {
        var result = await CategoryService.Categories();
        if (result.Value != null)
        {
            _availableCategories = result.Value;
        }
    }
}