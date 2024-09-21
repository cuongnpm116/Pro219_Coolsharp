using CustomerWebApp.Service.Customer;
using CustomerWebApp.Service.Customer.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Utilities;

namespace CustomerWebApp.Controllers;

public class AccountController : Controller
{
    private readonly ICustomerService _userService;
    private readonly IConfiguration _configuration;

    public AccountController(ICustomerService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("/register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("/auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        Result<bool> registerResult = await _userService.Register(model);
        if (!registerResult.Value)
        {
            return BadRequest(registerResult.Message);
        }
        LoginInfo info = new()
        {
            Username = model.Username,
            Password = model.Password,
        };
        Result<string> loginResult = await _userService.Login(info);
        if (!loginResult.IsSuccess || string.IsNullOrWhiteSpace(loginResult.Value))
        {
            return BadRequest(loginResult.Message);
        }

        ClaimsPrincipal principal = TokenUtility
            .GetPrincipalFromToken(loginResult.Value, _configuration["Jwt:SecretKey"]);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Ok("/");
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

    [HttpGet("/is-unique-email")]
    public async Task<bool> CheckUniqueEmail([FromQuery] string email)
    {
        bool result = await _userService.CheckUniqueEmail(email);
        return result;
    }

    [HttpGet("/is-unique-username")]
    public async Task<bool> CheckUniqueUsername([FromQuery] string username)
    {
        bool result = await _userService.CheckUniqueUsername(username);
        return result;
    }
}
