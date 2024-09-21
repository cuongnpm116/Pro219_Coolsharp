using MediatR;

namespace Application.Cqrs.Role.Create;
public class CreateRoleCommand : IRequest<bool>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
