using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Product.Vms.Create;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Product.Create;
public partial class ProductDetailForm
{
    [EditorRequired]
    [Parameter]
    public ProductDetailVm ProductDetail { get; set; }

    [EditorRequired]
    [Parameter]
    public IEnumerable<ColorForSelectVm> Colors { get; set; }

    [EditorRequired]
    [Parameter]
    public IEnumerable<SizeForSelectVm> Sizes { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<ColorForSelectVm> OnColorChanged { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<ProductDetailVm> OnDetailRemove { get; set; }

    private MudForm _form;
    private ProductDetailVmValidator _validator = new();

    // được tạo ra để gọi ở trong component cha
    public async Task<bool> ValidateAsync()
    {
        await _form.Validate();
        return _form.IsValid;
    }

    private async Task RemoveDetailClick()
    {
        await OnDetailRemove.InvokeAsync(ProductDetail);
    }

    private async Task ChangeColor(ColorForSelectVm color)
    {
        ProductDetail.Color = color;
        await OnColorChanged.InvokeAsync(color);
    }

    private void OnStockLoseFocus()
    {
        if (ProductDetail.Stock < 0)
        {
            ProductDetail.Stock = 0;
        }
    }

    private async Task<IEnumerable<ColorForSelectVm>> SearchColors(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return await Task.FromResult(Colors);
        }
        return await Task.FromResult(Colors.Where(x => x.Name.Contains(value, StringComparison.CurrentCultureIgnoreCase)));
    }

    private async Task<IEnumerable<SizeForSelectVm>> SearchSizes(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return await Task.FromResult(Sizes);
        }
        return await Task.FromResult(Sizes.Where(x => x.SizeNumber.ToString().Contains(value)));
    }
}