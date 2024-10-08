﻿namespace CustomerWebApp.Service.Address.Dtos;

public class AddAddressRequest
{
    public Guid CreatedBy { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty;
    public string DistrictCode { get; set; } = string.Empty;
    public string WardCode { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}
