using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;
using Users.API.Services;

namespace Users.API.Controllers;

[ApiController]
[Route("v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest registerUser) 
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _userService.RegisterUserAsync(registerUser);

        if (result.Success)
            return Ok(result);
        else if (result.Errors.Any())
            return BadRequest(result);

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> LoginUser([FromBody] UserLoginRequest userLogin) 
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _userService.LogInUserAsync(userLogin);

        if (result.Success)
            return Ok(result);

        return Unauthorized(result);
    }

    [Authorize]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<UserLoginResponse>> RefreshLoginUser() 
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return BadRequest();

        var result = await _userService.RefreshLogInAsync(userId);

        if (result.Success)
            return Ok(result);

        return Unauthorized();
    }
}
