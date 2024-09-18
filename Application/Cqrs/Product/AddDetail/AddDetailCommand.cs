using Application.Cqrs.Product.UpdateDetailWithNewImages;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.AddDetail;
public class AddDetailCommand : IRequest<Result<bool>>
{
    public Guid ProductId { get; set; }
    public UpdateDetailDto Detail { get; set; }
}
