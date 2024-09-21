using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Update;
public record UpdateSizeCommand(Guid Id, int SizeNumber, Status Status)
       : IRequest<Result>;