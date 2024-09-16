using FluentValidation;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Services.Product.Vms.Create;
public class ProductDetailVm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public ColorForSelectVm Color { get; set; }
    public SizeForSelectVm Size { get; set; }
}

public class ProductDetailVmValidator : AbstractValidator<ProductDetailVm>
{
    public ProductDetailVmValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThan(0).WithMessage("Phải lớn hơn 0");
        RuleFor(x => x.Price)
            .GreaterThan(1000).WithMessage("Phải lớn hơn 1000")
            .GreaterThan(x => x.OriginalPrice).WithMessage("Phải lớn hơn giá gốc");
        RuleFor(x => x.OriginalPrice)
            .GreaterThan(1000).WithMessage("Phải lớn hơn 1000");
        RuleFor(x => x.Color)
            .NotNull().WithMessage("Hãy chọn màu cho biến thể");
        RuleFor(x => x.Size)
            .NotNull().WithMessage("Hãy chọn kích cỡ cho biến thể");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propName) =>
    {
        if (propName.StartsWith("_validate"))
        {
            propName = propName.Replace("_validate", "");
        }

        var context = ValidationContext<ProductDetailVm>
            .CreateWithOptions((ProductDetailVm)model, x => x.IncludeProperties(propName));
        var result = await ValidateAsync(context);
        return result.IsValid ? [] : result.Errors.Select(x => x.ErrorMessage);
    };
}

