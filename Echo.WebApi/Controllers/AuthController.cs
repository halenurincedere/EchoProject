using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Echo.Business.Operations.User;
using Echo.Business.Dtos;
using Echo.Business.Operations.User.Dtos;
using Echo.Business.Jwt;
using Echo.WebAPI.Models;
using Echo.Data.Enums;

namespace Echo.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    // 🔐 Register a new user
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            return BadRequest("First name and last name are required.");

        var addUserDto = new AddUserDto
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            UserRole = UserRole.User // 👥 Default role assigned
        };

        var result = await _userService.AddUserAsync(addUserDto);

        if (result.IsSucceed)
            return Ok(new { message = result.Message });

        return BadRequest(new { error = result.Message });
    }

    // 🔓 Login and return JWT token
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.LoginUserAsync(new LoginUserDto
        {
            Email = request.Email,
            Password = request.Password
        });

        if (!result.IsSucceed)
            return BadRequest(new { error = result.Message });

        var user = result.Data;

        var token = JwtHelper.GenerateJwtToken(new JwtDto
        {
            Id            = user.Id,
            Email         = user.Email,
            FirstName     = user.FirstName,
            LastName      = user.LastName,
            UserRole      = user.UserRole,
            SecretKey     = _configuration["Jwt:SecretKey"]!,
            Issuer        = _configuration["Jwt:Issuer"]!,
            Audience      = _configuration["Jwt:Audience"]!,
            ExpireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!)
        });

        return Ok(new LoginResponse
        {
            Message = "Login successful.",
            Token = token
        });
    }

    // 🔎 Get current user info from token
    [HttpGet("me")]
    [Authorize] // Only accessible with a valid JWT
    public IActionResult GetMyUser()
    {
        // In a real scenario, we can extract user info from HttpContext.User.Claims
        return Ok(new { message = "Token is valid. You are authenticated." });
    }
}