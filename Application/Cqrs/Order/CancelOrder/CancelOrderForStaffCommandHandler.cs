using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.CancelOrder;

internal sealed class CancelOrderForStaffCommandHandler : IRequestHandler<CancelOrderForStaffCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    public CancelOrderForStaffCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result> Handle(CancelOrderForStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.CancelOrderForStaff(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}