using Microsoft.AspNetCore.Mvc;
using Moq;
using Users.API.Controllers;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;
using Users.API.Services;
using Users.Tests.Fixtures;

namespace Users.Tests.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void GetTest_Status_1_ReturnStatusCode200()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var userController = new UserController(userServiceMock.Object);
            int status = 1;

            // Act
            var result = (OkObjectResult) userController.Get(status);

            // Assert
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void GetTest_Status_2_ReturnBadRequest()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var userController = new UserController(userServiceMock.Object);
            int status = 2;

            // Act
            var result = (BadRequestObjectResult)userController.Get(status);

            // Assert
            Assert.Equal(result.StatusCode, 400);
        }

        [Fact]
        public void GetTest_Status_0_ReturnStatusCode500()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var userController = new UserController(userServiceMock.Object);
            int status = 0;

            // Act
            var result = (StatusCodeResult)userController.Get(status);

            // Assert
            Assert.Equal(result?.StatusCode, 500);
        }

        [Fact]
        public async Task RegisterUser_ValidUser_ReturnOk()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(u => u.RegisterUserAsync(RegisterUserFixtures.ValidUser))
                .ReturnsAsync(new RegisterUserResponse(true, UserFixtures.User));

            userServiceMock.Setup(u => u.GenerateEmailVerificationCode(UserFixtures.User))
                .ReturnsAsync(RegisterUserFixtures.SuccessEmailVerification);

            userServiceMock.Setup(u => u.SendConfirmationEmail("", ""))
                .ReturnsAsync(RegisterUserFixtures.ResponseSuccess);

            var userController = new UserController(userServiceMock.Object);

            // Act
            var response = await userController.RegisterUser(RegisterUserFixtures.ValidUser);

            var result = (OkObjectResult) response.Result;
            var resultValue = (RegisterUserResponse) result.Value;

            // Assert
            Assert.Equal(result?.StatusCode, 200);
            Assert.Equal(resultValue?.Success, true);
        }

        [Fact]
        public async Task RegisterUser_InvalidRequest_WrongPassword_ReturnBadRequest()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            var invalidRequest = new RegisterUserRequest
            {
                Email = "",
                Password = "123",
                PasswordConfirmation = "321"
            };

            var userController = new UserController(userServiceMock.Object);

            // Act
            var response = await userController.RegisterUser(invalidRequest);

            var result = (BadRequestObjectResult)response.Result;

            // Assert
            Assert.Equal(result?.StatusCode, 400);
        }

        [Fact]
        public async Task RegisterUser_InvalidUser_ReturnBadRequest()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            var failResponse = new RegisterUserResponse(false);
            failResponse.Errors = new List<string> { "", "", "" };

            userServiceMock.Setup(u => u.RegisterUserAsync(RegisterUserFixtures.ValidUser))
                .ReturnsAsync(failResponse);

            var userController = new UserController(userServiceMock.Object);

            // Act
            var response = await userController.RegisterUser(RegisterUserFixtures.ValidUser);

            var result = (BadRequestObjectResult)response.Result;
            var resultValue = (RegisterUserResponse)result.Value;

            // Assert
            Assert.Equal(result?.StatusCode, 400);
            Assert.Equal(resultValue?.Success, false);
        }
    }
}
