using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Statisticals;

public record TopProductQuery : IRequest<Result<List<OrderDetailVm>>>;