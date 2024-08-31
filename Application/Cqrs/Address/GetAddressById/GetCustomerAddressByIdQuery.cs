
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetAddressById;

public readonly record struct GetCustomerAddressByIdQuery(Guid AddressId) : IRequest<Result>;
