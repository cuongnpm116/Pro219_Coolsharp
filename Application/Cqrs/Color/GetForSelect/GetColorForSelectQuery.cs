using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.GetForSelect;
public readonly record struct GetColorForSelectQuery
    : IRequest<Result<IReadOnlyList<ColorForSelectVm>>>;
