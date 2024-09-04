namespace CustomerWebApp.Service.Address.Dtos;

public readonly record struct DeleteAddressRequest(Guid AddressId, Guid DeletedBy);

