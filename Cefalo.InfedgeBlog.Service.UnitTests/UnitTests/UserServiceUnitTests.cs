using AutoMapper;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using FakeItEasy;
using Cefalo.InfedgeBlog.Service.Services;
using Xunit;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Api.UnitTests.FakeData;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using System;

namespace Cefalo.InfedgeBlog.Service.UnitTests
{
    public class UserServiceUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly IUserRepository fakeUserRepository;
        private readonly IMapper fakeMapper;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IDateTimeHandler fakeDateTimeHandler;
        private readonly DtoValidatorBase<UserPostDto> fakeUserPostDtoValidator;
        private readonly DtoValidatorBase<UserUpdateDto> fakeUserUpdateDtoValidator;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser;
        private readonly UserDto fakeUserDto;
        private readonly UserPostDto fakeUserPostDto;
        private readonly UserUpdateDto fakeUserUpdateDto;
        public UserServiceUnitTests()
        {
            fakeUserRepository = A.Fake<IUserRepository>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeDateTimeHandler = A.Fake<IDateTimeHandler>();
            fakeUserPostDtoValidator = A.Fake<UserPostDtoValidator>();
            fakeUserUpdateDtoValidator = A.Fake<UserUpdateDtoValidator>();
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeUserPostDto = fakeUserData.fakeUserPostDto;
            fakeUserUpdateDto = fakeUserData.fakeUserUpdateDto;

            fakeUserService = new UserService(
                fakeUserRepository,
                fakeMapper,
                fakeJwtTokenHandler,
                fakeDateTimeHandler,
                fakeUserPostDtoValidator,
                fakeUserUpdateDtoValidator
            );
        }

        #region GetUserByIdAsync

        private void InitialCallForGetUserByIdAsync()
        {
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(fakeUser.Id)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }

        [Fact]
        public async void GetUserByIdAsync_WithValidParameter_UserRepositoryGetStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForGetUserByIdAsync();

            // Act
            var userDto = await fakeUserService.GetUserByIdAsync(fakeUser.Id);

            // Assert
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(fakeUser.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUserByIdAsync_WithValidParameter_MapperMapToUserDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForGetUserByIdAsync();

            // Act
            var userDto = await fakeUserService.GetUserByIdAsync(fakeUser.Id);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUserByIdAsync_WithValidParameter_ReturnsUserDtoCorrectly()
        {
            // Arrange
            InitialCallForGetUserByIdAsync();

            // Act
            var userDto = await fakeUserService.GetUserByIdAsync(fakeUser.Id);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(fakeUserDto, userDto);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithNonExistingId_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(nonExistingUserId)).Returns((User)null!);

            // Act
            Func<Task> act = async () => await fakeUserService.GetUserByIdAsync(nonExistingUserId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        #endregion

        #region GetUserByUsernameAsync

        private void InitialCallForGetUserByUsernameAsync()
        {
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }

        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_UserRepositoryGetUserByUsernameAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForGetUserByUsernameAsync();

            // Act
            var userDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);

            // Assert
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_MapperMapToUserDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForGetUserByUsernameAsync();

            // Act
            var userDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUserByUsernameAsync_WithValidParameter_ReturnsUserDtoCorrectly()
        {
            // Arrange
            InitialCallForGetUserByUsernameAsync();

            // Act
            var userDto = await fakeUserService.GetUserByUsernameAsync(fakeUser.Username);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(fakeUserDto, userDto);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WithNonExistingUsername_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingUsername = "nonExistingUsername";
            A.CallTo(() => fakeUserRepository.GetUserByUsernameAsync(nonExistingUsername)).Returns((User)null!);

            // Act
            Func<Task> act = async () => await fakeUserService.GetUserByUsernameAsync(nonExistingUsername);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        #endregion

        #region PostUserAsync

        private void InitialCallForPostUserAsync()
        {
            A.CallTo(() => fakeUserPostDtoValidator.ValidateDto(fakeUserPostDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeMapper.Map<User>(fakeUserPostDto)).Returns(fakeUser);
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).Returns(DateTime.UtcNow);
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }

        [Fact]
        public async void PostUserAsync_WithValidParameter_UserPostDtoValidatorValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeUserPostDtoValidator.ValidateDto(fakeUserPostDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostUserAsync_WithValidParameter_JwtTokenHandlerIsTokenExpiredIsCalledOnce()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostUserAsync_WithValidParameter_MapperMapToUserIsCalledOnce()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeUserPostDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostUserAsync_WithValidParameter_DateTimeHandlerGetCurrentUtcTimeIsCalledThreeTimes()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).MustHaveHappened(3, Times.Exactly);
        }

        [Fact]
        public async Task PostUserAsync_WithValidParameter_UserRepositoryPostUserAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeUserRepository.PostUserAsync(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostUserAsync_WithValidParameter_MapperMapToUserDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostUserAsync_WithValidParameter_ReturnsCreatedUserDtoCorrectly()
        {
            // Arrange
            InitialCallForPostUserAsync();

            // Act
            var userDto = await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(userDto, fakeUserDto);
        }

        [Fact]
        public async Task PostUserAsync_WithInvalidParameter_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeUserPostDtoValidator.ValidateDto(fakeUserPostDto)).Throws(new BadRequestException("Invalid user post request"));

            // Act
            Func<Task> act = async () => await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task PostUserAsync_WhenTokenExpired_ThrowsUnauthorizedException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> action = async () => await fakeUserService.PostUserAsync(fakeUserPostDto);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(action);
        }

        #endregion

        #region UpdateUserByIdAsync

        private void InitialCallForUpdateUserByIdAsync()
        {
            A.CallTo(() => fakeUserUpdateDtoValidator.ValidateDto(fakeUserUpdateDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(fakeUser.Id)).Returns(fakeUser);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(1);
            A.CallTo(() => fakeMapper.Map<User>(fakeUserUpdateDto)).Returns(fakeUser);
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).Returns(DateTime.UtcNow);
            A.CallTo(() => fakeUserRepository.UpdateUserByIdAsync(fakeUser.Id, fakeUser)).Returns(fakeUser);
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).Returns(fakeUserDto);
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_UserUpdateDtoValidatorValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateUserByIdAsync();

            // Act
            var userDto = await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            A.CallTo(() => fakeUserUpdateDtoValidator.ValidateDto(fakeUserUpdateDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_MapperMapsUserUpdateDtoToUser()
        {
            // Arrange
            InitialCallForUpdateUserByIdAsync();

            // Act
            var userDto = await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<User>(fakeUserUpdateDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_RepositoryUpdateUserByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateUserByIdAsync();

            // Act
            var userDto = await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            A.CallTo(() => fakeUserRepository.UpdateUserByIdAsync(fakeUser.Id, fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_MapperMapsUpdatedUserToUserDto()
        {
            // Arrange
            InitialCallForUpdateUserByIdAsync();

            // Act
            var userDto = await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<UserDto>(fakeUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_ReturnsUserDto()
        {
            // Arrange
            InitialCallForUpdateUserByIdAsync();

            // Act
            var userDto = await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(userDto, fakeUserDto);
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithExpiredToken_ThrowsUnauthorizedException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> action = async () => await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(action);
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithNonExistingUserId_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(nonExistingUserId)).Returns((User)null!);

            // Act
            Func<Task> action = async () => await fakeUserService.UpdateUserByIdAsync(nonExistingUserId, fakeUserUpdateDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async void UpdateUserByIdAsync_WithDifferentLoggedInUserId_ThrowsForbiddenException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(2);

            // Act
            Func<Task> action = async () => await fakeUserService.UpdateUserByIdAsync(fakeUser.Id, fakeUserUpdateDto);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        #endregion

        #region DeleteUserByIdAsync

        private void InitialCallForDeleteUserByIdAsync()
        {
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(fakeUser.Id)).Returns(fakeUser);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(1); 
            A.CallTo(() => fakeUserRepository.DeleteUserByIdAsync(fakeUser.Id)).Returns(true);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            // Arrange
            InitialCallForDeleteUserByIdAsync();

            // Act
            var result = await fakeUserService.DeleteUserByIdAsync(fakeUser.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WithExpiredToken_ThrowsUnauthorizedException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> action = async () => await fakeUserService.DeleteUserByIdAsync(fakeUser.Id);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(action);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WithNonExistingUser_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserRepository.GetUserByIdAsync(nonExistingUserId)).Returns((User)null!);

            // Act
            Func<Task> action = async () => await fakeUserService.DeleteUserByIdAsync(nonExistingUserId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WithDifferentLoggedInUserId_ThrowsForbiddenException()
        {
            // Arrange
            var fakeLoggedInUserId = 2;
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(fakeLoggedInUserId);

            // Act
            Func<Task> action = async () => await fakeUserService.DeleteUserByIdAsync(fakeUser.Id);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        #endregion

    }
}
