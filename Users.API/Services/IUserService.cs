using Microsoft.AspNetCore.Identity;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;
using Users.API.Models;

namespace Users.API.Services
{
    public interface IUserService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest registerUser);
        Task<EmailVerificationModel> GenerateEmailVerificationCode(IdentityUser user);
        Task<UserLoginResponse> LogInUserAsync(UserLoginRequest userLogin);
        Task<UserLoginResponse> RefreshLogInAsync(string userId);
        Task<RegisterUserResponse> SendConfirmationEmail(string email, string callbackUrl);
        Task<EmailConfirmationResponse> ConfirmUserEmailAsync(string userId, string verificationCode);
    }
}