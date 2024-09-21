using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.CancelOrder;
public record CancelOrderCommand(Guid OrderId, Guid ModifiedBy) : IRequest<Result>;