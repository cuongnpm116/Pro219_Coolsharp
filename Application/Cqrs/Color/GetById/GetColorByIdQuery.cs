using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.GetById;

public record GetColorByIdQuery(Guid coloId) : IRequest<Result>;

