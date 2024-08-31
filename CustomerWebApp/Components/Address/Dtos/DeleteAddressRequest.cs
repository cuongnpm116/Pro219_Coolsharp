namespace CustomerWebApp.Components.Address.Dtos;

public readonly record struct DeleteAddressRequest(Guid AddressId, Guid DeletedBy);

