using MediatR;

namespace Application.Cqrs.Role.UniqueCode;
public record CheckUniqueRoleCodeQuery(string Code) : IRequest<bool>;
