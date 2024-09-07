using System.Text.Json.Serialization;
using FluentValidation;
using StaffWebApp.Services.Category.ViewModel;

namespace StaffWebApp.Services.Product.Vms;
public class CreateProductInfoVm
{
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public CategoryVm _validateCategories { get; set; }
    public IEnumerable<CategoryVm> Categories { get; set; } = [];
}

public class CreateProductInfoVmValidator : AbstractValidator<CreateProductInfoVm>
{
    public CreateProductInfoVmValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Không được để trống");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("Hãy chọn ít nhất một danh mục sản phẩm");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        => async (model, propertyName) =>
    {
        if (propertyName.StartsWith("_validate"))
        {
            propertyName = propertyName.Replace("_validate", "");
        }
        var result = await ValidateAsync(
            ValidationContext<CreateProductInfoVm>.CreateWithOptions(
                (CreateProductInfoVm)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid ? (IEnumerable<string>)[] : result.Errors.Select(e => e.ErrorMessage);
    };
}
