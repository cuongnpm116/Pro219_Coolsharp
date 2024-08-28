using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetAddresses;
public readonly record struct GetAddressesQuery(Guid UserId) : IRequest<Result>;
