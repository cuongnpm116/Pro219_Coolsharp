

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.DeleteAddress;

public readonly record struct DeleteAddressCommand(Guid AddressId, Guid DeletedBy) : IRequest<Result>;
