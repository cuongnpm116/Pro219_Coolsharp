using FluentValidation;

namespace StaffWebApp.Services.Staff.Vms;

public class UpdateStaffInfoVm
{
    public Guid StaffId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
}

public class UpdateStaffInfoVmValidator : AbstractValidator<UpdateStaffInfoVm>
{
    public UpdateStaffInfoVmValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Tên không được để trống");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Họ không được để trống");

        RuleFor(x => x.DateOfBirth)
             .LessThan(DateTime.Now).WithMessage("Ngày sinh không thể ở tương lai")
             .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Ngày sinh không hợp lệ")
             .Must(x => IsAtLeast18YearsOld((DateTime)x)).WithMessage("Phải đủ 18 tuổi trở lên");
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
            ValidationContext<UpdateStaffInfoVm>
            .CreateWithOptions((UpdateStaffInfoVm)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid ? (IEnumerable<string>)[] : result.Errors.Select(e => e.ErrorMessage);
    };
}
