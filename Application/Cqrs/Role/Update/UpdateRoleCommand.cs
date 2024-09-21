using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.Update;
public record UpdateRoleCommand(Guid Id, string Code, string Name)
    : IRequest<Result>;
