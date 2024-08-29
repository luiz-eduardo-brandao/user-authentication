using Microsoft.AspNetCore.Identity;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;
using Users.API.Models;

namespace Users.Tests.Fixtures
{
    public static class RegisterUserFixtures
    {
        public static RegisterUserRequest ValidUser = new RegisterUserRequest
        {
            Email = "test@gmail.com",
            Password = "123@Test",
            PasswordConfirmation = "123@Test"
        };

        public static RegisterUserResponse ResponseSuccess = new RegisterUserResponse
        {
            Success = true,
            User = new IdentityUser
            {
                Email = "test@gmail.com"
            }
        };

        public static EmailVerificationModel SuccessEmailVerification = new EmailVerificationModel
        {
            Success = true,
            Email = "test@gmail.com",
            UserId = "123",
            VerificationCode = "123434fefgefe"
        };
    }
}
