using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.GetById;

public record GetSizeByIdQuery(Guid sizeId) : IRequest<Result>;

