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
        try
        {
            if (!ModelState.IsValid)
            return BadRequest();

            var result = await _userService.RegisterUserAsync(registerUser);

            if (result.Success) 
            {
                var emailVerification = await _userService.GenerateEmailVerificationCode(result.User);

                if (!emailVerification.Success)
                    return BadRequest(emailVerification);

                var callbackUrl = GenerateCallbackUrl(emailVerification.UserId, emailVerification.VerificationCode);

                var response = await _userService.SendConfirmationEmail(emailVerification.Email, callbackUrl);

                return Ok(result);
            }
            else if (result.Errors.Any())
                return BadRequest(result);

            return StatusCode(StatusCodes.Status500InternalServerError);   
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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

    [HttpGet("confirm-email")]
    public async Task<ActionResult<EmailConfirmationResponse>> ConfirmUserEmail(string userId, string code) 
    {
        var result = await _userService.ConfirmUserEmailAsync(userId, code);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    private string GenerateCallbackUrl(string userId, string code) 
    {
        return Request.Scheme + "://" + Request.Host + Url.Action("ConfirmUserEmail", "User", new { userId, code } );
    }
}
