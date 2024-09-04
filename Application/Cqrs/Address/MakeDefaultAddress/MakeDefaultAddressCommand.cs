using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.MakeDefaultAddress;

public readonly record struct MakeDefaultAddressCommand(
    Guid CurrentDefaultAddressId,
    Guid NewDefaultAddressId) : IRequest<Result>;