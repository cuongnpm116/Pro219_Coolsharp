using FluentValidation;
using StaffWebApp.Services.Role.Vms;

namespace StaffWebApp.Services.Staff.Vms;
public class AddStaffVm
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public RoleVmForGetAll _validateRoles { get; set; }
    public IEnumerable<RoleVmForGetAll> Roles { get; set; } = [];
}

public class AddStaffVmValidator : AbstractValidator<AddStaffVm>
{
    public AddStaffVmValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Không được để trống")
            .MaximumLength(50).WithMessage("Tối đa 50 ký tự");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Không được để trống")
            .MaximumLength(70).WithMessage("Tối đa 70 ký tự");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Ngày sinh không thể ở tương lai")
            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Ngày sinh không hợp lệ")
            .Must(x => IsAtLeast18YearsOld((DateTime)x)).WithMessage("Phải đủ 18 tuổi trở lên");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Không được để trống")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email không đúng định dạng");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Không được để trống")
            .MaximumLength(30).WithMessage("Tối đa 30 ký tự");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Không được để trống")
            .MinimumLength(8).WithMessage("Tối thiểu 8 ký tự")
            .MaximumLength(40).WithMessage("Tối đa 40 ký tự");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("Hãy chọn ít nhất 1 vai trò");
    }

    private bool IsAtLeast18YearsOld(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        => async (model, propertyName) =>
    {
        if (propertyName.StartsWith("_validate"))
        {
            propertyName = propertyName.Replace("_validate", "");
        }
        var result = await ValidateAsync(
            ValidationContext<AddStaffVm>
            .CreateWithOptions((AddStaffVm)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid ? (IEnumerable<string>)[] : result.Errors.Select(e => e.ErrorMessage);
    };
}

