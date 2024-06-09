using Microsoft.AspNetCore.Identity;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest registerUser) 
        {
            var user = new IdentityUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password); 

            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(user, false);

            var registerResponse = new RegisterUserResponse(result.Succeeded);

            if (!result.Succeeded && result.Errors.Any())
                registerResponse.AddErrors(result.Errors.Select(e => e.Description));

            return registerResponse;
        }

        public async Task<UserLoginResponse> LogInUserAsync(UserLoginRequest userLogin) 
        {
            return new UserLoginResponse(true);
        }
    }
}