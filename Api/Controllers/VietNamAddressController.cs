using Microsoft.AspNetCore.Mvc;
using VietNamAddress.Repos;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VietNamAddressController : ControllerBase
{
    private readonly IVietNamAddressRepository _vietnamAddressRepo;

    public VietNamAddressController(IVietNamAddressRepository vietnamAddressRepo)
    {
        _vietnamAddressRepo = vietnamAddressRepo;
    }

    [HttpGet("province")]
    public async Task<IActionResult> GetProvinces()
    {
        var result = await _vietnamAddressRepo.GetListProvince();
        return Ok(result);
    }

    [HttpGet("district")]
    public async Task<IActionResult> GetDistricts([FromQuery] string provinceCode)
    {
        var result = await _vietnamAddressRepo.GetListDistrict(provinceCode);
        return Ok(result);
    }

    [HttpGet("ward")]
    public async Task<IActionResult> GetWards([FromQuery] string districtCode)
    {
        var result = await _vietnamAddressRepo.GetListWard(districtCode);
        return Ok(result);
    }
}
