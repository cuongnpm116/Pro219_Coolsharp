namespace WebAppIntegrated.AddressService;
public readonly record struct MakeDefaultAddressModel(
    Guid CurrentDefaultAddressId,
    Guid NewDefaultAddressId);
