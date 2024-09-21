using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetOrderDetailForCustomer;

internal sealed class GetOrderDetailForCustomerQueryHandler : IRequestHandler<GetOrderDetailForCustomerQuery, Result>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderDetailForCustomerQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result> Handle(GetOrderDetailForCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.GetOrderDetails(request.OrderId);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
