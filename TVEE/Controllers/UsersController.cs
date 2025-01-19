using Microsoft.AspNetCore.Mvc;
using TVEEAPI.Models;
using TVEEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TVEEAPI.Helpers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UsersController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(AuthRequestDto request)
    {
        var user = await _userService.RegisterAsync(request.Name, request.Email, request.Password);
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
        return Ok(userDto);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateProfile(UserProfileUpdateDto updateDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var updatedUser = await _userService.UpdateProfileAsync(userId, updateDto);

        return Ok(new UserDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,
            Role = updatedUser.Role
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role
        }).ToList();
        return Ok(userDtos);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (userId != id && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = await _userService.DeleteAccountAsync(id);
        if (result)
            return NoContent();
        return NotFound();
    }
}
