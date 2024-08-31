

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.AddAddress;

public class AddCustomerAddressCommand : IRequest<Result>
{
    public Guid CreatedBy { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string ProvinceCode { get; init; } = string.Empty;
    public string DistrictCode { get; init; } = string.Empty;
    public string WardCode { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
}
