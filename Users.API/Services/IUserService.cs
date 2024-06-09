using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;

namespace Users.API.Services
{
    public interface IUserService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest registerUser);
        Task<UserLoginResponse> LogInUserAsync(UserLoginRequest userLogin);
    }
}