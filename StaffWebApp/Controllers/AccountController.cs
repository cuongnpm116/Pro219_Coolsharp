using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppIntegrated.Utilities;
using StaffWebApp.Services.Staff;
using StaffWebApp.Services.Staff.Requests;
using WebAppIntegrated.ApiResponse;

namespace StaffWebApp.Controllers;

public class AccountController : Controller
{
    private readonly IStaffService _userService;
    private readonly IConfiguration _configuration;

    public AccountController(IStaffService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("/login")]
    public IActionResult Login()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }
        return View();
    }

    [HttpPost("/auth/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginInfo info)
    {
        Result<string> result = await _userService.Login(info);

        if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.Value))
        {
            return BadRequest(result.Message);
        }

        ClaimsPrincipal principal = TokenUtility
            .GetPrincipalFromToken(result.Value, _configuration["Jwt:SecretKey"]);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Ok("/");
    }

    [HttpGet("/auth/logout")]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}
