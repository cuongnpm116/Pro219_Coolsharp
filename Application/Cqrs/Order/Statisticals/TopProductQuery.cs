using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Statisticals;

public class TopProductQuery : IRequest<Result<List<OrderDetailVm>>>
{
    public int Stock { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
}