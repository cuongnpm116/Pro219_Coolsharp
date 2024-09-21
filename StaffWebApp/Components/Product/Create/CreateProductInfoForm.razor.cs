using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Category.ViewModel;
using StaffWebApp.Services.Product.Vms.Create;

namespace StaffWebApp.Components.Product.Create;
public partial class CreateProductInfoForm
{
    [EditorRequired]
    [Parameter]
    public IEnumerable<CategoryVm> Categories { get; set; } = [];

    public ProductInfoVm _product { get; set; } = new();

    private MudForm _form;
    private MudSelect<CategoryVm> _categorySelect;
    private readonly ProductInfoVmValidator _validator = new();

    public async Task<bool> ValidateAsync()
    {
        await _form.Validate();
        return _form.IsValid;
    }

    private async Task CategorySelectionChanged(IEnumerable<CategoryVm> values)
    {
        _product.Categories = values;
        await _categorySelect.Validate();
    }
}