using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public record RegisterRequest(string Username, string Password, string Role);

    public record LoginRequest(string Username, string Password);

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var token = await _authService.RegisterAsync(
            request.Username,
            request.Password,
            request.Role
        );

        if (token == null)
            return BadRequest("Username Already Taken.");

        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Username, request.Password);

        if (token == null)
            return BadRequest("Invalid Email or Password.");

        return Ok(new { token });
    }
}
