using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetById;

public class GetOrderFroStaffByIdQueryHandler : IRequestHandler<GetOrderFroStaffByIdQuery, Result>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrderFroStaffByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<Result> Handle(GetOrderFroStaffByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Result<OrderVm> result = await _orderRepository.GetOrderDetailForStaff(request.OrderId);

            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

}