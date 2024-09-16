using FluentValidation;
using StaffWebApp.Services.Category.ViewModel;
using System.Text.Json.Serialization;

namespace StaffWebApp.Services.Product.Vms.Create;
public class ProductInfoVm
{
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public CategoryVm _validateCategories { get; set; }
    public IEnumerable<CategoryVm> Categories { get; set; } = [];

    public ProductInfoVm()
    {
    }

    public ProductInfoVm(string name, IEnumerable<CategoryVm> categories)
    {
        Name = name;
        Categories = categories;
    }
}

public class ProductInfoVmValidator : AbstractValidator<ProductInfoVm>
{
    public ProductInfoVmValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Không được để trống tên sản phẩm");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("Hãy chọn ít nhất 1 danh mục cho sản phẩm");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propName) =>
    {
        if (propName.StartsWith("_validate"))
        {
            propName = propName.Replace("_validate", "");
        }
        var context = ValidationContext<ProductInfoVm>
            .CreateWithOptions((ProductInfoVm)model, x => x.IncludeProperties(propName));
        var result = await ValidateAsync(context);
        return result.IsValid ? [] : result.Errors.Select(x => x.ErrorMessage);
    };
}

