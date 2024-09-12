using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Category;

namespace StaffWebApp.Components.Category;
public partial class CreateCategoryDialog
{
    [CascadingParameter]
    public MudDialogInstance DialogInstance { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;

    private string _categoryName = string.Empty;

    private async Task OnAddButtonClick()
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Xác nhận",
            $"Bạn chắc chắn muốn thêm danh mục {_categoryName}",
            yesText: "Thêm",
            cancelText: "Hủy");
        string validate = await ValidateCategoryName(_categoryName);
        if (!string.IsNullOrEmpty(validate))
        {
            Snackbar.Add("Thêm danh mục thất bại", Severity.Error);
            return;
        }
        bool result = await CategoryService.CreateCategory(_categoryName);
        if (result)
        {
            Snackbar.Add("Thêm danh mục thành công", Severity.Success);
            DialogInstance.Close(DialogResult.Ok(result));
        }
        else
        {
            Snackbar.Add("Thêm danh mục thất bại", Severity.Error);
            DialogInstance.Close(DialogResult.Cancel());
        }
    }

    private async Task<string> ValidateCategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Hãy nhập tên danh mục";
        }
        bool result = await CategoryService.CheckCategoryNameExist(value);
        if (result)
        {
            return "Tên danh mục đã tồn tại";
        }

        return string.Empty;
    }
}