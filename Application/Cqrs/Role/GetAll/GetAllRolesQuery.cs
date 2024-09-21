using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetAll;
public readonly record struct GetAllRolesQuery() : IRequest<Result>;
