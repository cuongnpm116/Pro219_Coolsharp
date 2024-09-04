using FluentValidation;

namespace CustomerWebApp.Service.Address.Dtos;

public class AddressAddOrEditVmValidator : AbstractValidator<AddressAddOrEditVm>
{
    public AddressAddOrEditVmValidator()
    {
        RuleFor(x => x.Province)
            .Must(x => !string.IsNullOrWhiteSpace(x.Code))
            .WithMessage("Tỉnh/Thành phố không được để trống");

        RuleFor(x => x.District)
            .Must(x => !string.IsNullOrWhiteSpace(x.Code))
            .WithMessage("Quận/Huyện không được để trống");

        RuleFor(x => x.Ward)
            .Must(x => !string.IsNullOrWhiteSpace(x.Code))
            .WithMessage("Xã/Thị trấn không được để trống");

        RuleFor(x => x.Detail).NotEmpty().WithMessage("Địa chỉ chi tiết không được để trống");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được để trống")
            .Matches(@"^\+?[0-9]{10,12}$").WithMessage("Số điện thoại không hợp lệ");
    }

    // phải là object rồi ép kiểu mới hoạt động. 
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        => async (model, propertyName) =>
        {
            var result = await ValidateAsync(
                ValidationContext<AddressAddOrEditVm>.CreateWithOptions(
                    (AddressAddOrEditVm)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
            {
                return [];
            }
            return result.Errors.Select(e => e.ErrorMessage);
        };
}
