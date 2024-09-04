using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.UpdateOrder;

public record UpdateOrderForStaffCommand(Guid Id, OrderStatus OrderStatus) : IRequest<Result>;

