using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.UpdateProductInfo;
public class UpdateProductInfoCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> CategoryIds { get; set; } = [];
}
