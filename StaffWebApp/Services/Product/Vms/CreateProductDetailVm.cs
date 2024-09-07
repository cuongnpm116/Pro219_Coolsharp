using FluentValidation;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Services.Product.Vms;
public class CreateProductDetailVm
{
    public Guid Id { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public ColorForSelectVm Color { get; set; }
    public SizeForSelectVm Size { get; set; }
}

public class CreateProductDetailVmValidator : AbstractValidator<CreateProductDetailVm>
{
    public CreateProductDetailVmValidator()
    {
        RuleFor(x => x.Price)
           .GreaterThan(x => x.OriginalPrice).WithMessage("Giá bán phải cao hơn giá gốc");

        RuleFor(x => x.Color)
            .Must(x => x.Id != Guid.Empty).WithMessage("Hãy chọn màu cho chi tiết sản phẩm");

        RuleFor(x => x.Size)
            .Must(x => x.Id != Guid.Empty).WithMessage("Hãy chọn kích cỡ cho chi tiết sản phẩm");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
      {
          var result = await ValidateAsync(
              ValidationContext<CreateProductDetailVm>
              .CreateWithOptions(
                  (CreateProductDetailVm)model, x => x.IncludeProperties(propertyName)));
          return result.IsValid ? (IEnumerable<string>)[] : result.Errors.Select(e => e.ErrorMessage);
      };

}
