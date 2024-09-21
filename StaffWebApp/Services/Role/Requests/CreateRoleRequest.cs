using FluentValidation;

namespace StaffWebApp.Services.Role.Requests;
public class CreateRoleRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    private readonly IRoleService _roleService;
    public CreateRoleRequestValidator(IRoleService roleService)
    {
        _roleService = roleService;
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã vai trò không được để trống")
            .MustAsync(IsUniqueCode).WithMessage("Mã vai trò đã tồn tại");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên vai trò không được để trống")
            .MustAsync(IsUniqueName).WithMessage("Tên vai trò đã tồn tại");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propName) =>
    {
        var context = ValidationContext<CreateRoleRequest>
            .CreateWithOptions((CreateRoleRequest)model, x => x.IncludeProperties(propName));
        var result = await ValidateAsync(context);
        return result.IsValid ? [] : result.Errors.Select(x => x.ErrorMessage);
    };

    private async Task<bool> IsUniqueCode(string code, CancellationToken token)
    {
        var result = await _roleService.CheckUniqueRoleCode(code);
        return !result;
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var result = await _roleService.CheckUniqueRoleName(name);
        return !result;
    }
}
