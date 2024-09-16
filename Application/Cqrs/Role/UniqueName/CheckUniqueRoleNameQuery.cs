using MediatR;

namespace Application.Cqrs.Role.UniqueName;
public record CheckUniqueRoleNameQuery(string Name) : IRequest<bool>;
