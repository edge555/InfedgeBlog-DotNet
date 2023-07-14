using Cefalo.InfedgeBlog.Api.Controllers;
using Cefalo.InfedgeBlog.Api.UnitTests.FakeData;
using Cefalo.InfedgeBlog.Api.Utils.Pagination.Filter;
using Cefalo.InfedgeBlog.Api.Utils.Pagination.Helpers;
using Cefalo.InfedgeBlog.Api.Utils.Pagination.Wrappers;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Cefalo.InfedgeBlog.Api.UnitTests
{
    public class UserControllerUnitTests
    {
        private readonly IUserService fakeUserService;
        private readonly UserController fakeUserController;
        private readonly FakeUserData fakeUserData;
        private readonly UserDto fakeUserDto;
        private readonly List<UserDto> fakeUserDtoList;
        private readonly UserPostDto fakeUserPostDto;
        private readonly UserUpdateDto fakeUserUpdateDto;
        private readonly PaginationFilter fakePaginationFilter;
        public UserControllerUnitTests()
        {
            fakeUserService = A.Fake<IUserService>();
            fakeUserController = new UserController(fakeUserService);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUserDto = fakeUserData.fakeUserDto;
            fakeUserDtoList = fakeUserData.fakeUserDtoList;
            fakeUserPostDto = fakeUserData.fakeUserPostDto;
            fakeUserUpdateDto = fakeUserData.fakeUserUpdateDto;
            fakePaginationFilter = new PaginationFilter
            {
                PageNumber = 1,
                PageSize = 10
            };
        }
        #region GetUsersAsync
        [Fact]
        public async void GetUsersAsync_GetUsersAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeUserService.GetUsersAsync(1, 10)).Returns(fakeUserDtoList);

            // Act
            var myUserList = await fakeUserController.GetUsersAsync(fakePaginationFilter);

            // Assert
            A.CallTo(() => fakeUserService.GetUsersAsync(1, 10)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUsersAsync_WithValidParameter_ReturnsUserListCorrectly()
        {
            // Arrange
            var fakeTotalRecords = 20;
            
            A.CallTo(() => fakeUserService.GetUsersAsync(fakePaginationFilter.PageNumber, fakePaginationFilter.PageSize)).Returns(fakeUserDtoList);
            A.CallTo(() => fakeUserService.CountUsersAsync()).Returns(fakeTotalRecords);
            var expectedPagedResponse = PaginationHelper.CreatePagedReponse(fakeUserDtoList, fakePaginationFilter, fakeTotalRecords);
            // Act
            var result = await fakeUserController.GetUsersAsync(fakePaginationFilter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResponse = okResult.Value.Should().BeAssignableTo<PagedResponse<List<UserDto>>>().Subject;
            pagedResponse.Data.Should().BeEquivalentTo(expectedPagedResponse.Data);
            pagedResponse.PageNumber.Should().Be(expectedPagedResponse.PageNumber);
            pagedResponse.PageSize.Should().Be(expectedPagedResponse.PageSize);
            pagedResponse.TotalPages.Should().Be(expectedPagedResponse.TotalPages);
            pagedResponse.TotalRecords.Should().Be(expectedPagedResponse.TotalRecords);
        }

        [Fact]
        public async void GetUsersAsync_WithEmptyUserList_ReturnsEmptyList()
        {
            // Arrange
            var fakeEmptyUserDtoList = new List<UserDto>();
            var fakeTotalRecords = 0;
            
            A.CallTo(() => fakeUserService.GetUsersAsync(fakePaginationFilter.PageNumber, fakePaginationFilter.PageSize)).Returns(fakeEmptyUserDtoList);
            A.CallTo(()=> fakeUserService.CountUsersAsync()).Returns(fakeTotalRecords);
            var expectedPagedResponse = PaginationHelper.CreatePagedReponse(fakeEmptyUserDtoList, fakePaginationFilter, fakeTotalRecords);
            
            // Act
            var result = await fakeUserController.GetUsersAsync(fakePaginationFilter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResponse = okResult.Value.Should().BeAssignableTo<PagedResponse<List<UserDto>>>().Subject;
            pagedResponse.Data.Should().BeEmpty();
            pagedResponse.PageNumber.Should().Be(expectedPagedResponse.PageNumber);
            pagedResponse.PageSize.Should().Be(expectedPagedResponse.PageSize);
            pagedResponse.TotalPages.Should().Be(expectedPagedResponse.TotalPages);
            pagedResponse.TotalRecords.Should().Be(expectedPagedResponse.TotalRecords);

        }
        #endregion

        #region GetUserByIdAsync
        [Fact]
        public async void GetUserByIdAsync_WithValidParameter_GetUserByIdAsyncAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeUserService.GetUserByIdAsync(fakeUserDto.Id)).Returns(fakeUserDto);

            // Act
            var userDto = await fakeUserController.GetUserByIdAsync(fakeUserDto.Id);

            //Assert
            A.CallTo(() => fakeUserService.GetUserByIdAsync(fakeUserDto.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetUserByIdAsync_WithValidParameter_ReturnsUserDtoCorrectly()
        {
            // Arrange
            int existingUserId = 1;
            A.CallTo(() => fakeUserService.GetUserByIdAsync(existingUserId)).Returns(fakeUserDto);
            var controller = new UserController(fakeUserService);

            // Act
            var result = await controller.GetUserByIdAsync(existingUserId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var userDto = Assert.IsAssignableFrom<UserDto>(okObjectResult.Value);

            userDto.Should().NotBeNull();
            userDto.Id.Should().Be(existingUserId);
            userDto.Username.Should().Be("edge555");
            userDto.Name.Should().Be("Shoaib Ahmed");
            okObjectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void GetUserByIdAsync_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserService.GetUserByIdAsync(nonExistingUserId)).Returns(Task.FromResult<UserDto>(null));
            
            // Act
            var result = await fakeUserController.GetUserByIdAsync(nonExistingUserId);
            
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        #endregion

        #region PostUserAsync
        [Fact]
        public async void PostUserAsync_WithValidParameter_PostUserAsyncIsCalledOnce()
        {
            //Arrange
            A.CallTo(() => fakeUserService.PostUserAsync(fakeUserPostDto)).Returns(fakeUserDto);
            
            //Act
            var newUser = await fakeUserController.PostUserAsync(fakeUserPostDto);
            
            //Assert
            A.CallTo(() => fakeUserService.PostUserAsync(fakeUserPostDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void PostUserAsync_WithValidParameter_ReturnsCreatedUserCorrectly()
        {
            //Arrange
            A.CallTo(() => fakeUserService.PostUserAsync(fakeUserPostDto)).Returns(fakeUserDto);
            
            //Act
            var newUser = await fakeUserController.PostUserAsync(fakeUserPostDto);
            
            //Assert
            newUser.Should().NotBeNull();
            newUser.Should().BeOfType<CreatedResult>();
            var newUserObject = (CreatedResult)newUser;
            newUserObject.Value.Should().BeEquivalentTo(fakeUserDto);
            newUserObject.StatusCode.Should().Be(201);
        }
        [Fact]
        public async Task PostUserAsync_WithEmptyList_ReturnsNoContent()
        {
            // Arrange
            A.CallTo(() => fakeUserService.PostUserAsync(fakeUserPostDto)).Returns((UserDto)null);
            var controller = new UserController(fakeUserService);

            // Act
            var result = await controller.PostUserAsync(fakeUserPostDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.StatusCode.Should().Be(400);
        }
        #endregion

        #region UpdateUserByIdAsync
        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_UpdateUserAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeUserService.UpdateUserByIdAsync(fakeUserDto.Id, fakeUserUpdateDto)).Returns(fakeUserDto);
            
            // Act
            var updatedUserDto = await fakeUserController.UpdateUserByIdAsync(fakeUserDto.Id, fakeUserUpdateDto);
            
            // Assert
            A.CallTo(() => fakeUserService.UpdateUserByIdAsync(fakeUserDto.Id, fakeUserUpdateDto)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void UpdateUserByIdAsync_WithValidParameter_ReturnsUpdatedUserCorrectly()
        {
            // Arrange
            int existingUserId = 1;
            var updatedUserDtoFromService = new UserDto { Id = existingUserId, Name = "Ahmed Shoaib"};
            
            A.CallTo(() => fakeUserService.UpdateUserByIdAsync(existingUserId, fakeUserUpdateDto)).Returns(updatedUserDtoFromService);
            var controller = new UserController(fakeUserService);

            // Act
            var result = await controller.UpdateUserByIdAsync(existingUserId, fakeUserUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedUser = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.Equal(existingUserId, updatedUser.Id);
            Assert.Equal(fakeUserUpdateDto.Name, updatedUser.Name);
        }
        [Fact]
        public async Task UpdateUserById_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserService.UpdateUserByIdAsync(nonExistingUserId, fakeUserUpdateDto)).Returns(Task.FromResult<UserDto>(null));

            // Act
            var result = await fakeUserController.UpdateUserByIdAsync(nonExistingUserId, fakeUserUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Can not update user.", badRequestResult.Value);
        }
        [Fact]
        public async Task UpdateUserByIdAsync_WithInvalidParameter_ReturnsBadRequest()
        {
            // Arrange
            int existingUserId = 1;
            var invalidUserUpdateDto = new UserUpdateDto { Name = "Ahmed Shoaib", Password = "abcd1234" };

            A.CallTo(() => fakeUserService.UpdateUserByIdAsync(existingUserId, invalidUserUpdateDto)).Returns((UserDto)null);
            var controller = new UserController(fakeUserService);

            // Act
            var result = await controller.UpdateUserByIdAsync(existingUserId, invalidUserUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Can not update user.", errorMessage);
        }
        #endregion

        #region DeleteUserByIdAsync
        [Fact]
        public async void DeleteUserAsync_WithValidParameter_DeleteUserAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeUserService.DeleteUserByIdAsync(fakeUserDto.Id)).Returns(true);
            
            // Act
            var isDeleted = await fakeUserController.DeleteUserByIdAsync(fakeUserDto.Id);
            
            // Assert
            A.CallTo(() => fakeUserService.DeleteUserByIdAsync(fakeUserDto.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void DeleteUserByIdAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            // Arrange
            A.CallTo(() => fakeUserService.DeleteUserByIdAsync(fakeUserDto.Id)).Returns(true);
            
            // Act
            var isDeleted = await fakeUserController.DeleteUserByIdAsync(fakeUserDto.Id);

            // Assert
            isDeleted.Should().NotBeNull();
            isDeleted.Should().BeOfType<NoContentResult>();
            var isDeletedObject = (NoContentResult)isDeleted;
            isDeletedObject.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void DeleteUserByIdAsync_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingUserId = -1;
            A.CallTo(() => fakeUserService.DeleteUserByIdAsync(fakeUserDto.Id)).Returns(false);

            // Act
            var result = await fakeUserController.DeleteUserByIdAsync(nonExistingUserId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;
            badRequestResult.StatusCode.Should().Be(400);
        }
        #endregion
    }
}
