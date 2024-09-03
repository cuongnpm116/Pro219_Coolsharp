using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Get;

internal class GetOrdersForStaffQueryHandler : IRequestHandler<GetOrdersForStaffQuery, Result<PaginationResponse<OrderVm>>>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrdersForStaffQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PaginationResponse<OrderVm>>> Handle(GetOrdersForStaffQuery request, CancellationToken cancellationToken)
    {
        Result<PaginationResponse<OrderVm>> result = await _orderRepository.GetOrdersForStaff(request);
        return result;
    }


}