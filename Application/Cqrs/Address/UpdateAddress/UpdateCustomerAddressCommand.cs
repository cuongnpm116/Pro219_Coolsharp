using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.UpdateAddress;

public readonly record struct UpdateCustomerAddressCommand(
    Guid ModifiedBy,
    Guid Id,
    string ProvinceCode,
    string PhoneNumber,
    string DistrictCode,
    string WardCode,
    string Detail,
    bool IsDefault) : IRequest<Result>;
