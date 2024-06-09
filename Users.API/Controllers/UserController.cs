using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Users.API.DTOs.Requests;
using Users.API.Services;

namespace Users.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUser) 
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
    public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest userLogin) 
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _userService.LogInUserAsync(userLogin);

        if (result.Success)
            return Ok(result);

        return Unauthorized(result);
    }

}
