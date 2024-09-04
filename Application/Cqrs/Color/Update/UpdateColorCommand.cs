using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Update;
public record UpdateColorCommand(Guid Id, string Name, Status Status)
     : IRequest<Result>;