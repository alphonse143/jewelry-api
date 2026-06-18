using Microsoft.AspNetCore.Mvc;
using JewelryAPI.Application.DTOs;
using JewelryAPI.Application.Interfaces;

namespace JewelryAPI.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        if (response == null) return Unauthorized(new { message = "Invalid username or password" });

        return Ok(response);
    }
}
