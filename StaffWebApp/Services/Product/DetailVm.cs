﻿using FluentValidation;
using StaffWebApp.Services.Color.Vms;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Services.Product;

public class DetailVm
{
    public Guid Id { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ColorForSelectVm Color { get; set; }
    public SizeForSelectVm Size { get; set; }
}

public class DetailVmValidator : AbstractValidator<DetailVm>
{
    public DetailVmValidator()
    {
        RuleFor(x => x.OriginalPrice)
            .GreaterThan(1000).WithMessage("Phải lớn hơn 1000");

        RuleFor(x => x.Price)
            .GreaterThan(1000).WithMessage("Phải lớn hơn 1000")
            .GreaterThan(x => x.OriginalPrice).WithMessage("Phải lớn hơn giá gốc");

        RuleFor(x => x.Color)
            .NotNull().WithMessage("Hãy chọn màu cho biến thể");

        RuleFor(x => x.Size)
            .NotNull().WithMessage("Hãy chọn kích cỡ cho biến thể");
    }

    public async Task<IEnumerable<string>> ValidateValueAsync(object model, string propertyName)
    {
        var context = ValidationContext<DetailVm>
            .CreateWithOptions((DetailVm)model, x => x.IncludeProperties(propertyName));
        var result = await ValidateAsync(context);
        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    }
}
