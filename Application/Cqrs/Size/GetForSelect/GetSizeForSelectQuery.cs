using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.GetForSelect;
public readonly record struct GetSizeForSelectQuery()
    : IRequest<Result<IReadOnlyList<SizeForSelectVm>>>;
