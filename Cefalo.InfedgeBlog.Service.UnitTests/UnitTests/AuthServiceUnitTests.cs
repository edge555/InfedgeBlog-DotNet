using AutoMapper;
using Cefalo.InfedgeBlog.Api.UnitTests.FakeData;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Cefalo.InfedgeBlog.Service.Services;
using FakeItEasy;
using Xunit;

namespace Cefalo.InfedgeBlog.Service.UnitTests
{
    public class AuthServiceUnitTests
    {
        private readonly IAuthService fakeAuthService;
        private readonly IUserRepository fakeUserRepository;
        private readonly IMapper fakeMapper;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IDateTimeHandler fakeDateTimeHandler;
        private readonly IPasswordHandler fakePasswordHandler;
        private readonly DtoValidatorBase<SignupDto> fakeSignupDtoValidator;
        private readonly DtoValidatorBase<LoginDto> fakeLoginDtoValidator;
        private readonly User fakeUser;
        private readonly FakeUserData fakeUserData;
        private readonly SignupDto fakeSignupDto;
        private readonly LoginDto fakeLoginDto;
        private readonly UserDto fakeUserDto;
        private readonly UserWithTokenDto fakeUserWithTokenDto;

        public AuthServiceUnitTests()
        {
            fakeUserRepository = A.Fake<IUserRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeDateTimeHandler = A.Fake<IDateTimeHandler>();
            fakePasswordHandler = A.Fake<IPasswordHandler>();
            fakeLoginDtoValidator = A.Fake<LoginDtoValidator>();
            fakeSignupDtoValidator = A.Fake<SignupDtoValidator>();
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeSignupDto = fakeUserData.fakeSignupDto;
            fakeLoginDto = fakeUserData.fakeLoginDto;
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeUserWithTokenDto = fakeUserData.fakeUserWithTokenDto;
            fakeAuthService = new AuthService(
                fakeUserRepository,
                fakeMapper,
                fakeJwtTokenHandler,
                fakeDateTimeHandler,
                fakePasswordHandler,
                fakeSignupDtoValidator,
                fakeLoginDtoValidator
            );
        }

        #region SignupAsync

        private void InitialCallForSignupAsync()
        {
            A.CallTo(() => fakeSignupDtoValidator.ValidateDto(fakeSignupDto)).DoesNothing();
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeSignupDto.Username)).Returns((User)null!);
            A.CallTo(() => fakeUserRepository.GetUserByEmailAsync(fakeSignupDto.Email)).Returns((User)null!);
            A.CallTo(() => fakeMapper.Map<User>(fakeSignupDto)).Returns(fakeUser);
            A.CallTo(() => fakePasswordHandler.HashPassword(fakeSignupDto.Password)).Returns("HashedPassword");
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_ValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeSignupDtoValidator.ValidateDto(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_MapToUserIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeSignupDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_UserRepositoryGetUserByUsernameAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeSignupDto.Username)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_UserRepositoryGetUserByEmailAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeUserRepository.GetUserByEmailAsync(fakeSignupDto.Email)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_HashPasswordIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakePasswordHandler.HashPassword(fakeSignupDto.Password)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_UserRepositoryPostUserAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_MapToUserDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignupAsync_WithValidParameter_ReturnsCreatedUserDtoCorrectly()
        {
            // Arrange
            InitialCallForSignupAsync();

            // Act
            var createdUserDto = await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            Assert.NotNull(createdUserDto);
            Assert.Equal(fakeUserDto, createdUserDto);
        }

        [Fact]
        public async void SignupAsync_WithInvalidParameter_ReturnedBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeSignupDtoValidator.ValidateDto(fakeSignupDto)).Throws(new BadRequestException("Invalid signup request"));

            // Act
            Func<Task> action = async () => await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(action);
            Assert.Equal("Invalid signup request", exception.Message);
        }

        [Fact]
        public async Task SignupAsync_WhenUserExistsWithUsername_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeSignupDto.Username)).Returns(fakeUser);

            // Act
            Func<Task> action = async () => await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task SignupAsync_WhenUserExistsWithEmail_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeUserRepository.GetUserByEmailAsync(fakeSignupDto.Email)).Returns(fakeUser);

            // Act
            Func<Task> action = async () => await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task SignupAsync_WhenPostUserAsyncReturnsNull_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).Returns((User)null!);

            // Act
            Func<Task> action = async () => await fakeAuthService.SignupAsync(fakeSignupDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        #endregion

        #region LoginAsync

        private void InitialCallForLoginAsync()
        {
            A.CallTo(() => fakeLoginDtoValidator.ValidateDto(fakeLoginDto)).DoesNothing();
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeSignupDto.Username)).Returns(fakeUser);
            A.CallTo(() => fakePasswordHandler.Verify(fakeLoginDto.Password, fakeUser.Password)).Returns(true);
            A.CallTo(() => fakeMapper.Map<UserWithTokenDto>(fakeUser)).Returns(fakeUserWithTokenDto);
            A.CallTo(() => fakeJwtTokenHandler.GenerateJwtToken(fakeUser)).Returns("jwttoken");
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_ValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            A.CallTo(() => fakeLoginDtoValidator.ValidateDto(fakeLoginDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_UserRepositoryGetUserByUsernameAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeLoginDto.Username)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_PasswordHandlerVerifyIsCalledOnce()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            A.CallTo(() => fakePasswordHandler.Verify(fakeLoginDto.Password, fakeUser.Password)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_MapperMapToUserWithTokenDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserWithTokenDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_JwtTokenHandlerGenerateJwtTokenIsCalledOnce()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.GenerateJwtToken(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void LoginAsync_WithValidParameter_ReturnsUserWithTokenCorrectly()
        {
            // Arrange
            InitialCallForLoginAsync();

            // Act
            var userWithToken = await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            Assert.NotNull(userWithToken);
            Assert.Equal(fakeUserWithTokenDto, userWithToken);
        }

        [Fact]
        public async void LoginAsync_WithInvalidParameter_ReturnedBadRequestException()
        {
            // Arrange
            InitialCallForLoginAsync();
            A.CallTo(() => fakeLoginDtoValidator.ValidateDto(fakeLoginDto)).Throws(new BadRequestException("Invalid login request"));

            // Act
            Func<Task> action = async () => await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task LoginAsync_WhenNoUserExistsWithUsername_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeLoginDto.Username)).Returns((User)null!);

            // Act
            Func<Task> action = async () => await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordDoesNotMatch_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakePasswordHandler.Verify(fakeLoginDto.Password, fakeUser.Password)).Returns(false);

            // Act
            Func<Task> action = async () => await fakeAuthService.LoginAsync(fakeLoginDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        #endregion
    }
}
