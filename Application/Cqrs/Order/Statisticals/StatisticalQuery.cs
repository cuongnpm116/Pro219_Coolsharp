using Domain.Primitives;
using MediatR;


namespace Application.Cqrs.Order.Statisticals;

public record StatisticalQuery : IRequest<Result<List<OrderVm>>>;
