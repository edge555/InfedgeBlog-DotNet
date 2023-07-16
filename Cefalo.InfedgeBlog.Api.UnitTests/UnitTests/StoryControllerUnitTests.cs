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
    public class StoryControllerUnitTests
    {
        private readonly IStoryService fakeStoryService;
        private readonly StoryController fakeStoryController;
        private readonly FakeStoryData fakeStoryData;
        private readonly StoryDto fakeStoryDto;
        private readonly List<StoryDto> fakeStoryDtoList;
        private readonly StoryPostDto fakeStoryPostDto;
        private readonly StoryUpdateDto fakeStoryUpdateDto;
        private readonly PaginationFilter fakePaginationFilter;

        public StoryControllerUnitTests()
        {
            fakeStoryService = A.Fake<IStoryService>();
            fakeStoryController = new StoryController(fakeStoryService);
            fakeStoryData = new FakeStoryData();
            fakeStoryDto = fakeStoryData.fakeStoryDto;
            fakeStoryDtoList = fakeStoryData.fakeStoryDtoList;
            fakeStoryPostDto = fakeStoryData.fakeStoryPostDto;
            fakeStoryUpdateDto = fakeStoryData.fakeStoryUpdateDto;
            fakePaginationFilter = new PaginationFilter
            {
                PageNumber = 1,
                PageSize = 10
            };
        }

        #region GetStoriesAsync

        [Fact]
        public async Task GetStoriesAsync_GetStoriesAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1, 10)).Returns(fakeStoryDtoList);

            // Act
            var result = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);

            // Assert
            A.CallTo(() => fakeStoryService.GetStoriesAsync(1, 10)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetStoriesAsync_WithValidPaginationParameters_ReturnsCorrectPagedResponse()
        {
            // Arrange
            var fakeTotalRecords = 20;
            A.CallTo(() => fakeStoryService.GetStoriesAsync(fakePaginationFilter.PageNumber, fakePaginationFilter.PageSize)).Returns(fakeStoryDtoList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeTotalRecords);
            var expectedPagedResponse = PaginationHelper.CreatePagedReponse(fakeStoryDtoList, fakePaginationFilter, fakeTotalRecords);
            
            // Act
            var result = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResponse = okResult.Value.Should().BeAssignableTo<PagedResponse<List<StoryDto>>>().Subject;
            pagedResponse.Data.Should().BeEquivalentTo(expectedPagedResponse.Data);
            pagedResponse.PageNumber.Should().Be(expectedPagedResponse.PageNumber);
            pagedResponse.PageSize.Should().Be(expectedPagedResponse.PageSize);
            pagedResponse.TotalPages.Should().Be(expectedPagedResponse.TotalPages);
            pagedResponse.TotalRecords.Should().Be(expectedPagedResponse.TotalRecords);
        }

        [Fact]
        public async Task GetStoriesAsync_WithEmptyResult_ReturnsEmptyResponse()
        {
            // Arrange
            var fakeEmptyStoryDtoList = new List<StoryDto>();
            var fakeTotalRecords = 0;
            A.CallTo(() => fakeStoryService.GetStoriesAsync(fakePaginationFilter.PageNumber, fakePaginationFilter.PageSize)).Returns(fakeEmptyStoryDtoList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeTotalRecords);
            var expectedPagedResponse = PaginationHelper.CreatePagedReponse(fakeEmptyStoryDtoList, fakePaginationFilter, fakeTotalRecords);
            
            // Act
            var result = await fakeStoryController.GetStoriesAsync(fakePaginationFilter);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResponse = okResult.Value.Should().BeAssignableTo<PagedResponse<List<StoryDto>>>().Subject;
            pagedResponse.Data.Should().BeEmpty();
            pagedResponse.PageNumber.Should().Be(expectedPagedResponse.PageNumber);
            pagedResponse.PageSize.Should().Be(expectedPagedResponse.PageSize);
            pagedResponse.TotalPages.Should().Be(expectedPagedResponse.TotalPages);
            pagedResponse.TotalRecords.Should().Be(expectedPagedResponse.TotalRecords);
        }

        [Fact]
        public async Task GetStoriesAsync_WithExceededPageNumber_ReturnsEmptyResponse()
        {
            // Arrange
            var fakeTotalRecords = 20;
            var exceededPageNumber = 1000;
            var fakeEmptyStoryDtoList = new List<StoryDto>(); 
            A.CallTo(() => fakeStoryService.GetStoriesAsync(exceededPageNumber, fakePaginationFilter.PageSize)).Returns(fakeEmptyStoryDtoList);
            A.CallTo(() => fakeStoryService.CountStoriesAsync()).Returns(fakeTotalRecords);
            var expectedPagedResponse = PaginationHelper.CreatePagedReponse(fakeEmptyStoryDtoList, fakePaginationFilter, fakeTotalRecords); 
            
            // Act
            var result = await fakeStoryController.GetStoriesAsync(new PaginationFilter { PageNumber = exceededPageNumber, PageSize = fakePaginationFilter.PageSize });
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResponse = okResult.Value.Should().BeAssignableTo<PagedResponse<List<StoryDto>>>().Subject;
            pagedResponse.Data.Should().BeEmpty();
            pagedResponse.PageNumber.Should().Be(exceededPageNumber);
            pagedResponse.PageSize.Should().Be(expectedPagedResponse.PageSize);
            pagedResponse.TotalPages.Should().Be(expectedPagedResponse.TotalPages);
            pagedResponse.TotalRecords.Should().Be(expectedPagedResponse.TotalRecords);
        }
        #endregion

        #region GetStoryByIdAsync
        [Fact]
        public async Task GetStoryById_WithValidParameter_ReturnsOkResult()
        {
            // Arrange
            var existingStoryId = 1;
            var fakeStoryDto = fakeStoryData.fakeStoryDto;
            A.CallTo(() => fakeStoryService.GetStoryByIdAsync(existingStoryId)).Returns(fakeStoryDto);
            
            // Act
            var result = await fakeStoryController.GetStoryByIdAsync(existingStoryId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var story = Assert.IsAssignableFrom<StoryDto>(okResult.Value);
            Assert.Equal(existingStoryId, story.Id);
            Assert.Equal("Title1", story.Title);
            Assert.Equal("Body1", story.Body);
        }

        [Fact]
        public async Task GetStoryById_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingStoryId = -1;
            A.CallTo(() => fakeStoryService.GetStoryByIdAsync(nonExistingStoryId)).Returns((StoryDto)null!);
            
            // Act
            var result = await fakeStoryController.GetStoryByIdAsync(nonExistingStoryId);
            
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
        #endregion

        #region PostStoryAsync
        [Fact]
        public async void PostStoryAsync_WithValidParameter_PostStoryAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakeStoryPostDto)).Returns(fakeStoryDto);

            // Act
            var newStory = await fakeStoryController.PostStoryAsync(fakeStoryPostDto);

            // Assert
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakeStoryPostDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostStoryAsync_WithValidParameter_ReturnsCreatedStoryCorrectly()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.PostStoryAsync(fakeStoryPostDto)).Returns(fakeStoryDto);

            // Act
            var result = await fakeStoryController.PostStoryAsync(fakeStoryPostDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            var createdStory = Assert.IsAssignableFrom<StoryDto>(createdResult.Value);
            Assert.Equal(fakeStoryDto.Id, createdStory.Id);
            Assert.Equal(fakeStoryDto.Title, createdStory.Title);
            Assert.Equal(fakeStoryDto.Body, createdStory.Body);
        }

        [Fact]
        public async void PostStoryAsync_StoryPostWithoutBody_ReturnsBadRequest()
        {
            // Arrange
            var storyDtoWithoutBody = new StoryPostDto { Title = "Title1" };

            A.CallTo(() => fakeStoryService.PostStoryAsync(storyDtoWithoutBody)).Returns((StoryDto)null!);

            // Act
            var result = await fakeStoryController.PostStoryAsync(storyDtoWithoutBody);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Can not create story.", badRequestResult.Value);
        }

        [Fact]
        public async void PostStoryAsync_StoryPostWithoutTitle_ReturnsBadRequest()
        {
            // Arrange
            var storyDtoWithoutTitle = new StoryPostDto { Body = "Body1" };

            A.CallTo(() => fakeStoryService.PostStoryAsync(storyDtoWithoutTitle)).Returns((StoryDto)null!);

            // Act
            var result = await fakeStoryController.PostStoryAsync(storyDtoWithoutTitle);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Can not create story.", badRequestResult.Value);
        }

        #endregion

        #region UpdateStoryByIdAsync
        [Fact]
        public async void UpdateStoryByIdAsync_WithValidParameter_UpdateStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.UpdateStoryByIdAsync(fakeStoryDto.Id, fakeStoryUpdateDto)).Returns(fakeStoryDto);
            
            // Act
            var newStory = await fakeStoryController.UpdateStoryByIdAsync(fakeStoryDto.Id, fakeStoryUpdateDto);
            
            // Assert
            A.CallTo(() => fakeStoryService.UpdateStoryByIdAsync(fakeStoryDto.Id, fakeStoryUpdateDto)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateStoryByIdAsync_WithValidParameter_ReturnsUpdatedStoryCorrectly()
        {
            // Arrange
            var existingStoryId = 1;
            var updatedStoryDtoFromService = new StoryDto { Id = existingStoryId, Title = "Updated Title", Body = "Updated Body" };
            A.CallTo(() => fakeStoryService.UpdateStoryByIdAsync(existingStoryId, fakeStoryUpdateDto)).Returns(updatedStoryDtoFromService);

            // Act
            var result = await fakeStoryController.UpdateStoryByIdAsync(existingStoryId, fakeStoryUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedStory = Assert.IsAssignableFrom<StoryDto>(okResult.Value);
            Assert.Equal(existingStoryId, updatedStory.Id);
            Assert.Equal(fakeStoryUpdateDto.Title, updatedStory.Title);
            Assert.Equal(fakeStoryUpdateDto.Body, updatedStory.Body);
        }
        [Fact]
        public async void UpdateStoryByIdAsync_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingStoryId = -1;
            A.CallTo(() => fakeStoryService.UpdateStoryByIdAsync(nonExistingStoryId, fakeStoryUpdateDto)).Returns((StoryDto)null!);
            
            // Act
            var result = await fakeStoryController.UpdateStoryByIdAsync(nonExistingStoryId, fakeStoryUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Can not update story.", badRequestResult.Value);
        }
        #endregion

        #region DeleteStoryAsync
        [Fact]
        public async void DeleteStoryByIdAsync_WithValidParameter_DeleteStoryByIdAsyncIsCalledOnce()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.DeleteStoryByIdAsync(fakeStoryDto.Id)).Returns(true);
            
            // Act
            var isDeleted = await fakeStoryController.DeleteStoryByIdAsync(fakeStoryDto.Id);
            
            // Assert
            A.CallTo(() => fakeStoryService.DeleteStoryByIdAsync(fakeStoryDto.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async void DeleteStoryByIdAsync_WithValidParameter_ReturnsTrueIfDeleted()
        {
            // Arrange
            A.CallTo(() => fakeStoryService.DeleteStoryByIdAsync(fakeStoryDto.Id)).Returns(true);
            
            // Act
            var isDeleted = await fakeStoryController.DeleteStoryByIdAsync(fakeStoryDto.Id);
            
            // Assert
            isDeleted.Should().NotBeNull();
            isDeleted.Should().BeOfType<NoContentResult>();
            var isDeletedObject = (NoContentResult)isDeleted;
            isDeletedObject.StatusCode.Should().Be(204);
        }
        [Fact]
        public async void DeleteStoryByIdAsync_WithNonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var nonExistingStoryId = -1;
            A.CallTo(() => fakeStoryService.DeleteStoryByIdAsync(nonExistingStoryId)).Returns(false);

            // Act
            var result = await fakeStoryController.DeleteStoryByIdAsync(nonExistingStoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;
            badRequestResult.StatusCode.Should().Be(400);
        }
        #endregion
    }
}
