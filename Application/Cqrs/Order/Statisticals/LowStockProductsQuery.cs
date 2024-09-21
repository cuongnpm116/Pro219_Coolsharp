using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Statisticals;

public class LowStockProductsQuery() : IRequest<Result<List<ProductDetailForStaffVm>>>
{
}
