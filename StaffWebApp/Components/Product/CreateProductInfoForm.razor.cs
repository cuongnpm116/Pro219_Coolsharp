using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Category;
using StaffWebApp.Services.Category;
using StaffWebApp.Services.Category.ViewModel;
using StaffWebApp.Services.Product.Vms;

namespace StaffWebApp.Components.Product;
public partial class CreateProductInfoForm
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [EditorRequired]
    [Parameter]
    public CreateProductInfoVm CreateProductInfo { get; set; } = null!;

    private CreateProductInfoVmValidator _validator = new();
    private MudSelect<CategoryVm> _categorySelect;
    private MudForm _form;

    private IEnumerable<CategoryVm> _categories = [];

    protected override async Task OnInitializedAsync()
    {
        await GetCategories();
    }

    private async Task GetCategories()
    {
        var result = await CategoryService.Categories();
        _categories = result.Value ?? [];
    }

    private async Task CategorySelectionChanged(IEnumerable<CategoryVm> values)
    {
        CreateProductInfo.Categories = values;
        await _categorySelect.Validate();
    }

    public async Task<bool> ValidateProductInfoAsync()
    {
        await _form.Validate();
        return _form.IsValid;
    }

    private async Task ShowAddCategoryDialog()
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

        var dialog = await DialogService.ShowAsync<CreateCategoryDialog>("Thêm danh m?c", dialogOptions);
        var result = await dialog.Result;
        if (result.Data is true)
        {
            await GetCategories();
        }
    }
}