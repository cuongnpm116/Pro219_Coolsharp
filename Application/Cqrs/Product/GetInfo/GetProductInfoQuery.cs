using MediatR;

namespace Application.Cqrs.Product.GetInfo;
public record GetProductInfoQuery(Guid ProductId) : IRequest<ProductInfoDto>;
