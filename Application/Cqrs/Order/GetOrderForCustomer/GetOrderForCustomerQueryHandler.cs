using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetOrderForCustomer;

internal sealed class GetOrderForCustomerQueryHandler : IRequestHandler<GetOrderForCustomerQuery, Result>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrderForCustomerQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result> Handle(GetOrderForCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.GetOrdersForCustomer(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
