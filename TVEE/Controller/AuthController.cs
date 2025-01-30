using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Tvee.Models;
using Tvee.Services;
namespace Tvee.Controllers{

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // ✅ Login endpoint (No Authorization needed)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var token = await _authService.Login(loginDTO);
        if (token == null)
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(new { Token = token });
    }

    // ✅ Register endpoint (No Authorization needed)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var user = await _authService.Register(registerDTO);
        if (user == null)
        {
            return BadRequest("Registration failed.");
        }
        return Ok(user);
    }

    // ✅ Get user profile (Requires Authorization)
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Invalid token.");
        }

        var user = await _authService.GetUserProfile(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    // ✅ Update user profile (Requires Authorization)
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserDTO updateDTO)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Invalid token.");
        }

        var updatedUser = await _authService.UpdateUserProfile(userId, updateDTO);
        if (updatedUser == null)
        {
            return NotFound("User not found.");
        }

        return Ok(updatedUser);
    }

    // ✅ Delete user profile (Requires Authorization)
    [Authorize]
    [HttpDelete("profile")]
    public async Task<IActionResult> DeleteUserProfile()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Invalid token.");
        }

        bool deleted = await _authService.DeleteUserProfile(userId);
        if (!deleted)
        {
            return NotFound("User not found.");
        }

        return Ok("Profile deleted successfully.");
    }
}
}