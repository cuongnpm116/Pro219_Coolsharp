using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Statisticals;

internal sealed class TopProductQueryHandler : IRequestHandler<TopProductQuery, Result<List<OrderDetailVm>>>
{
    private readonly IOrderRepository _orderRepository;
    public TopProductQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<OrderDetailVm>>> Handle(TopProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.TopProducts(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result<List<OrderDetailVm>>.Error(ex.Message);
        }
    }
}
