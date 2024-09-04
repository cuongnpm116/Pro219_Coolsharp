using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetDefaultAddress;

public readonly record struct GetDefaultAddressQuery(Guid UserId) : IRequest<Result>;
