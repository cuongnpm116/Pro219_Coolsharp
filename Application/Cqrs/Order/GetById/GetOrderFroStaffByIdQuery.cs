using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetById;
public record GetOrderFroStaffByIdQuery(Guid OrderId) : IRequest<Result>;

