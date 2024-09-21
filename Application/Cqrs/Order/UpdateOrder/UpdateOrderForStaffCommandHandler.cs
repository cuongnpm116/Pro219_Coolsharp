

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.UpdateOrder;

internal sealed class UpdateOrderForStaffCommandHandler : IRequestHandler<UpdateOrderForStaffCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    public UpdateOrderForStaffCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result> Handle(UpdateOrderForStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.UpdateOrderStatusForStaff(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}