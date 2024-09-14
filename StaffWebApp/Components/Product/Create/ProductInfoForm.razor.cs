using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Category;
using StaffWebApp.Services.Category.ViewModel;
using StaffWebApp.Services.Product.Vms.Create;

namespace StaffWebApp.Components.Product.Create;
public partial class ProductInfoForm
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;

    public ProductInfoVm Product { get; set; } = new();

    private IEnumerable<CategoryVm> _categories = [];

    private MudForm _form;
    private MudSelect<CategoryVm> _categorySelect;
    private readonly ProductInfoVmValidator _validator = new();

    protected override async Task OnInitializedAsync()
    {
        await GetCategories();
    }

    public async Task<bool> ValidateAsync()
    {
        await _form.Validate();
        return _form.IsValid;
    }

    private async Task CategorySelectionChanged(IEnumerable<CategoryVm> values)
    {
        Product.Categories = values;
        await _categorySelect.Validate();
    }

    private async Task GetCategories()
    {
        var result = await CategoryService.Categories();
        if (result.Value != null)
        {
            _categories = result.Value;
        }
       
    }
}