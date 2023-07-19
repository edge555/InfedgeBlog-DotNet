using Cefalo.InfedgeBlog.Api.Controllers;
using Cefalo.InfedgeBlog.Api.UnitTests.FakeData;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cefalo.InfedgeBlog.Api.UnitTests
{
    public class AuthControllerUnitTests
    {
        private readonly IAuthService fakeAuthService;
        private readonly AuthController fakeAuthController;
        private readonly FakeUserData fakeUserData;
        private readonly UserDto fakeUserDto;
        private readonly SignupDto fakeSignupDto;
        private readonly LoginDto fakeLoginDto;
        private readonly UserWithTokenDto fakeUserWithTokenDto;
        private readonly ILogger<AuthController> fakeLogger;

        public AuthControllerUnitTests()
        {
            fakeAuthService = A.Fake<IAuthService>();
            fakeUserData = A.Fake<FakeUserData>();
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeSignupDto = fakeUserData.fakeSignupDto;
            fakeLoginDto = fakeUserData.fakeLoginDto;
            fakeUserWithTokenDto = fakeUserData.fakeUserWithTokenDto;
            fakeLogger = A.Fake<ILogger<AuthController>>();
            fakeAuthController = new AuthController(fakeAuthService, fakeLogger);
        }

        #region SignupAsync
        [Fact]
        public async void SignupAsync_WithValidParameter_SignupAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).Returns(fakeUserDto);
            
            // Act
            var newUser = await fakeAuthController.SignupAsync(fakeSignupDto);
            
            // Assert
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            // Arrange
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).Returns(fakeUserDto);
            
            // Act
            var result = await fakeAuthController.SignupAsync(fakeSignupDto);
            
            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<UserDto>>();
            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(fakeUserDto);
            createdResult.StatusCode.Should().Be(201);
        }

        [Fact]
        public async void SignupAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            // Arrange
            UserDto? nullUserDto = null;
            A.CallTo(() => fakeAuthService.SignupAsync(fakeSignupDto)).Returns(nullUserDto);
            
            // Act
            var result = await fakeAuthController.SignupAsync(fakeSignupDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.Value.Should().Be("Can not signup.");
            badRequestResult.StatusCode.Should().Be(400);
        }
        #endregion

        #region LoginAsync

        [Fact]
        public async void LoginAsync_WithValidParameter_LoginAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).Returns(fakeUserWithTokenDto);
            
            // Act
            var newUser = await fakeAuthController.LoginAsync(fakeLoginDto);
            
            // Assert
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_ReturnsLoggedinUserCorrectly()
        {
            // Arrange
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).Returns(fakeUserWithTokenDto);
            
            // Act
            var result = await fakeAuthController.LoginAsync(fakeLoginDto);
            
            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<UserWithTokenDto>>();
            var createdResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(fakeUserWithTokenDto);
            createdResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            // Arrange
            UserWithTokenDto? nullUserWithTokenDto = null;
            A.CallTo(() => fakeAuthService.LoginAsync(fakeLoginDto)).Returns(nullUserWithTokenDto);
            
            // Act
            var result = await fakeAuthController.LoginAsync(fakeLoginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.Value.Should().Be("Can not login.");
            badRequestResult.StatusCode.Should().Be(400);
        }

        #endregion
    }
}
