using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.CheckUpdateDetail;
public class CheckUpdateDetailQuery : IRequest<Result<Guid>>
{
    public Guid ProductId { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
}
