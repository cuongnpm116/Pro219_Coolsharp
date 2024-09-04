using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Create;
public record CreateSizeCommand(int SizeNumber, Status Status)
    : IRequest<Result>;
