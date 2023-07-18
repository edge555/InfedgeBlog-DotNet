using AutoMapper;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using FakeItEasy;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.UnitTests.FakeData;
using Cefalo.InfedgeBlog.Service.Services;
using Xunit;
using Cefalo.InfedgeBlog.Service.CustomExceptions;

namespace Cefalo.InfedgeBlog.Service.UnitTests
{
    public class StoryServiceUnitTests
    {
        private readonly IStoryRepository fakeStoryRepository;
        private readonly IStoryService fakeStoryService;
        private readonly IUserService fakeUserService;
        private readonly IMapper fakeMapper;
        private readonly IJwtTokenHandler fakeJwtTokenHandler;
        private readonly IDateTimeHandler fakeDateTimeHandler;
        private readonly DtoValidatorBase<StoryPostDto> fakeStoryPostDtoValidator;
        private readonly DtoValidatorBase<StoryUpdateDto> fakeStoryUpdateDtoValidator;
        private readonly FakeStoryData fakeStoryData;
        private readonly Story fakeStory;
        private readonly StoryDto fakeStoryDto;
        private readonly StoryPostDto fakeStoryPostDto;
        private readonly StoryUpdateDto fakeStoryUpdateDto;
        private readonly int fakePageNumber;
        private readonly int fakePageSize;
        public StoryServiceUnitTests()
        {
            fakeStoryRepository = A.Fake<IStoryRepository>();
            fakeUserService = A.Fake<IUserService>();
            fakeMapper = A.Fake<IMapper>();
            fakeJwtTokenHandler = A.Fake<IJwtTokenHandler>();
            fakeDateTimeHandler = A.Fake<IDateTimeHandler>();
            fakeStoryPostDtoValidator = A.Fake<StoryPostDtoValidator>();
            fakeStoryUpdateDtoValidator = A.Fake<StoryUpdateDtoValidator>();
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStory = fakeStoryData.fakeStory;
            fakeStoryDto = fakeStoryData.fakeStoryDto;
            fakeStoryPostDto = fakeStoryData.fakeStoryPostDto;
            fakeStoryUpdateDto = fakeStoryData.fakeStoryUpdateDto;
            fakePageNumber = 1;
            fakePageSize = 1;

            fakeStoryService = new StoryService(
                fakeStoryRepository,
                fakeUserService,
                fakeMapper,
                fakeJwtTokenHandler,
                fakeDateTimeHandler,
                fakeStoryPostDtoValidator,
                fakeStoryUpdateDtoValidator
            );
        }

        #region GetStoriesAsync

        private void InitialCallForGetStoriesAsync()
        {
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(fakePageNumber, fakePageSize)).Returns(fakeStoryData.fakeStoryList);
            A.CallTo(() => fakeMapper.Map<IEnumerable<StoryDto>>(fakeStoryData.fakeStoryList)).Returns(fakeStoryData.fakeStoryDtoList);
        }

        [Fact]
        public async void GetStoriesAsync_WithValidParameter_StoryRepositoryGetStoriesAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForGetStoriesAsync();

            // Act
            var storyList = await fakeStoryService.GetStoriesAsync(fakePageNumber, fakePageSize);

            // Assert
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(fakePageNumber, fakePageSize)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetStoriesAsync_WithValidParameter_MapperMapToStoryDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForGetStoriesAsync();

            // Act
            var storyList = await fakeStoryService.GetStoriesAsync(fakePageNumber, fakePageSize);

            // Assert
            A.CallTo(() => fakeMapper.Map<IEnumerable<StoryDto>>(fakeStoryData.fakeStoryList)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async void GetStoriesAsync_WithValidParameter_ReturnsStoryListCorrectly()
        {
            // Arrange
            InitialCallForGetStoriesAsync();

            // Act
            var storyList = await fakeStoryService.GetStoriesAsync(fakePageNumber, fakePageSize);

            // Assert
            Assert.NotNull(storyList);
            Assert.Equal(fakeStoryData.fakeStoryDtoList, storyList);
        }

        [Fact]
        public async Task GetStoriesAsync_WithValidParameter_ReturnsEmptyList()
        {
            // Arrange
            var emptyStoryList = new List<Story>();
            A.CallTo(() => fakeStoryRepository.GetStoriesAsync(fakePageNumber, fakePageSize)).Returns(Task.FromResult(emptyStoryList));
            A.CallTo(() => fakeMapper.Map<IEnumerable<StoryDto>>(emptyStoryList)).Returns(Enumerable.Empty<StoryDto>());

            // Act
            var storyList = await fakeStoryService.GetStoriesAsync(fakePageNumber, fakePageSize);

            // Assert
            Assert.NotNull(storyList);
            Assert.Empty(storyList);
        }

        #endregion

        #region GetStoryByIdAsync

        private void InitialCallForGetStoryByIdAsync()
        {
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).Returns(fakeStoryData.fakeStoryDto);
            A.CallTo(() => fakeUserService.GetUserByIdAsync(fakeStory.AuthorId)).Returns(new UserDto { Name = "AuthorName" });
        }

        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_StoryRepositoryGetStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForGetStoryByIdAsync();

            // Act
            var singleStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_MapperMapToStoryDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForGetStoryByIdAsync();

            // Act
            var singleStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_UserServiceGetUserByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForGetStoryByIdAsync();

            // Act
            var singleStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeUserService.GetUserByIdAsync(fakeStory.AuthorId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetStoryByIdAsync_WithValidParameter_ReturnsStoryDtoCorrectly()
        {
            // Arrange
            InitialCallForGetStoryByIdAsync();

            // Act
            var singleStory = await fakeStoryService.GetStoryByIdAsync(fakeStory.Id);

            // Assert
            Assert.NotNull(singleStory);
            Assert.Equal(fakeStoryData.fakeStoryDto, singleStory);
        }

        [Fact]
        public async Task GetStoryByIdAsync_WithNonExistingId_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingStoryId = -1;
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(nonExistingStoryId)).Returns((Story)null!);

            // Act
            Func<Task> act = async () => await fakeStoryService.GetStoryByIdAsync(nonExistingStoryId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        #endregion

        #region PostStoryAsync

        private void InitialCallForPostStoryAsync()
        {
            A.CallTo(() => fakeStoryPostDtoValidator.ValidateDto(fakeStoryPostDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeMapper.Map<Story>(fakeStoryPostDto)).Returns(fakeStory);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(1);
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).Returns(DateTime.UtcNow);
            A.CallTo(() => fakeStoryRepository.PostStoryAsync(fakeStory)).Returns(fakeStory);
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).Returns(fakeStoryDto);
        }

        [Fact]
        public async void PostStoryAsync_WithValidParameter_StoryPostDtoValidatorValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeStoryPostDtoValidator.ValidateDto(fakeStoryPostDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_JwtTokenHandlerIsTokenExpiredIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_MapperMapToStoryIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<Story>(fakeStoryPostDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_JwtTokenHandlerGetLoggedInUserIdIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_DateTimeHandlerGetCurrentUtcTimeIsCalledTwice()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_StoryRepositoryPostStoryAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStory = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeStoryRepository.PostStoryAsync(fakeStory)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostStoryAsync_WithValidParameter_MapperMapToStoryDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStoryDto = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostStoryAsync_WithValidParameter_ReturnsCreatedStoryDtoCorrectly()
        {
            // Arrange
            InitialCallForPostStoryAsync();

            // Act
            var createdStoryDto = await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            Assert.NotNull(createdStoryDto);
            Assert.Equal(createdStoryDto, fakeStoryDto);
        }

        [Fact]
        public async Task PostStoryAsync_WithInvalidParameter_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeStoryPostDtoValidator.ValidateDto(fakeStoryPostDto)).Throws(new BadRequestException("Invalid story post request"));

            // Act
            Func<Task> act = async () => await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task PostStoryAsync_WhenTokenExpired_ThrowsUnauthorizedException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> action = async () => await fakeStoryService.PostStoryAsync(fakeStoryPostDto);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(action);
        }

        #endregion

        #region UpdateStoryByIdAsync

        private void InitialCallForUpdateStoryByIdAsync()
        {
            A.CallTo(() => fakeStoryUpdateDtoValidator.ValidateDto(fakeStoryUpdateDto)).DoesNothing();
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(1);
            A.CallTo(() => fakeMapper.Map<Story>(fakeStoryUpdateDto)).Returns(fakeStory);
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).Returns(DateTime.UtcNow);
            A.CallTo(() => fakeStoryRepository.UpdateStoryByIdAsync(fakeStory.Id,fakeStory)).Returns(fakeStory);
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).Returns(fakeStoryDto);
        }

        [Fact]
        public async void UpdateStoryByIdAsync_WithValidParameter_StoryUpdateDtoValidatorValidateDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeStoryUpdateDtoValidator.ValidateDto(fakeStoryUpdateDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_JwtTokenHandlerIsTokenExpiredIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_StoryRepositoryGetStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_JwtTokenHandlerGetLoggedInUserIdIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_MapperMapToStoryIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<Story>(fakeStoryUpdateDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_DateTimeHandlerGetCurrentUtcTimeIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeDateTimeHandler.GetCurrentUtcTime()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_StoryRepositoryUpdateStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeStoryRepository.UpdateStoryByIdAsync(fakeStory.Id, fakeStory)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithValidParameter_MapperMapToStoryDtoIsCalledOnce()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            A.CallTo(() => fakeMapper.Map<StoryDto>(fakeStory)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateStoryByIdAsync_WithValidParameter_ReturnsUpdatedStoryDtoCorrectly()
        {
            // Arrange
            InitialCallForUpdateStoryByIdAsync();

            // Act
            var updatedStoryDto = await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            Assert.NotNull(updatedStoryDto);
            Assert.Equal(updatedStoryDto, fakeStoryDto);
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WithInvalidParameter_ThrowsBadRequestException()
        {
            // Arrange
            A.CallTo(() => fakeStoryUpdateDtoValidator.ValidateDto(fakeStoryUpdateDto)).Throws(new BadRequestException("Invalid story update request"));

            // Act
            Func<Task> act = async () => await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WhenTokenExpired_ThrowsUnauthorizedException()
        {
            // Arrange
            var invalidStoryUpdateDto = new StoryUpdateDto();
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> action = async () => await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, invalidStoryUpdateDto);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(action);
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WhenStoryNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingStoryId = -1;
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(nonExistingStoryId)).Returns((Story)null!);

            // Act
            Func<Task> action = async () => await fakeStoryService.UpdateStoryByIdAsync(nonExistingStoryId, fakeStoryUpdateDto);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task UpdateStoryByIdAsync_WhenNotAuthorized_ThrowsForbiddenException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(2); 

            // Act
            Func<Task> action = async () => await fakeStoryService.UpdateStoryByIdAsync(fakeStory.Id, fakeStoryUpdateDto);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        #endregion

        #region DeleteStoryByIdAsync
        private void InitialCallForDeleteStoryByIdAsync()
        {
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(false);
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns(fakeStory);
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(1);
            A.CallTo(() => fakeStoryRepository.DeleteStoryByIdAsync(fakeStory.Id)).Returns(true);
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WithValidParameter_JwtTokenHandlerIsTokenExpiredIsCalledOnce()
        {
            // Arrange
            InitialCallForDeleteStoryByIdAsync();

            // Act
            await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WithValidParameter_StoryRepositoryGetStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForDeleteStoryByIdAsync();

            // Act
            await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WithValidParameter_JwtTokenHandlerGetLoggedInUserIdIsCalledOnce()
        {
            // Arrange
            InitialCallForDeleteStoryByIdAsync();

            // Act
            await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WithValidParameter_StoryRepositoryDeleteStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            InitialCallForDeleteStoryByIdAsync();

            // Act
            await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            A.CallTo(() => fakeStoryRepository.DeleteStoryByIdAsync(fakeStory.Id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            // Arrange
            InitialCallForDeleteStoryByIdAsync();

            // Act
            var result = await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WhenTokenExpired_ThrowsUnauthorizedException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.IsTokenExpired()).Returns(true);

            // Act
            Func<Task> act = async () => await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WhenStoryNotFound_ThrowsNotFoundException()
        {
            // Arrange
            A.CallTo(() => fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id)).Returns((Story)null!);

            // Act
            Func<Task> act = async () => await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);
            
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task DeleteStoryByIdAsync_WhenNotAuthorized_ThrowsForbiddenException()
        {
            // Arrange
            A.CallTo(() => fakeJwtTokenHandler.GetLoggedInUserId()).Returns(2);

            // Act
            Func<Task> act = async () => await fakeStoryService.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(act);
        }

        #endregion
    }

}
