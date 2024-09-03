using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Create;
public record CreateColorCommand(string Name, Status Status)
    : IRequest<Result>;

