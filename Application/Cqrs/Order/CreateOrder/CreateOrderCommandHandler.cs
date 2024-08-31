using Application.IRepositories;
using Application.IServices;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    public CreateOrderCommandHandler(IOrderRepository orderRepository, IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
    }
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.AddOrder(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
            
        }
    }

}
