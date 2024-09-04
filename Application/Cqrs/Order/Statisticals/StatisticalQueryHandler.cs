using Application.IRepositories;
using Domain.Primitives;
using MediatR;


namespace Application.Cqrs.Order.Statisticals;

internal sealed class StatisticalQueryHandler : IRequestHandler<StatisticalQuery, Result<List<OrderVm>>>
{
    private readonly IOrderRepository _orderRepository;
    public StatisticalQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<OrderVm>>> Handle(StatisticalQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.Statistical();
            return result;
        }
        catch (Exception ex)
        {
            return Result<List<OrderVm>>.Error(ex.Message);
        }
    }
}