

using Application.Cqrs.Product;
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Statisticals;

internal class LowStockProductsQueryHandler : IRequestHandler<LowStockProductsQuery, Result<List<ProductDetailForStaffVm>>>
{
    private readonly IOrderRepository _orderRepository;
    public LowStockProductsQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<ProductDetailForStaffVm>>> Handle(LowStockProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderRepository.LowStockProducts();
            return result;

        }
        catch (Exception ex)
        {
            return Result<List<ProductDetailForStaffVm>>.Error(ex.Message);
        }
    }
}