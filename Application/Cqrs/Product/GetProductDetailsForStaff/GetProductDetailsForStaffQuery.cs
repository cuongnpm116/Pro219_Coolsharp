using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailsForStaff;
public record GetProductDetailsForStaffQuery(Guid ProductId)
    : IRequest<Result<IEnumerable<ProductDetailForStaffVm>>>;
