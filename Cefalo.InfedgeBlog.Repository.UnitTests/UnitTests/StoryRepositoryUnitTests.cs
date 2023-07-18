using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Repository.Repositories;
using Cefalo.InfedgeBlog.Repository.UnitTests.FakeData;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cefalo.InfedgeBlog.Repository.UnitTests
{
    public class StoryRepositoryUnitTests
    {
        private readonly ApplicationDbContext fakeDbContext;
        private readonly IStoryRepository fakeStoryRepository;
        private readonly FakeStoryData fakeStoryData;
        private readonly Story fakeStory, fakeUpdateStory;
        private readonly List<Story> fakeStoryList;
        public StoryRepositoryUnitTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "fakeDbtaContext").Options;
            fakeDbContext = new ApplicationDbContext(options);
            fakeStoryRepository = new StoryRepository(fakeDbContext);
            fakeStoryData = A.Fake<FakeStoryData>();
            fakeStory = fakeStoryData.fakeStory;
            fakeUpdateStory = fakeStoryData.fakeUpdateStory;
            fakeStoryList = fakeStoryData.fakeStoryList;
        }

        private async void ClearDatabase()
        {
            fakeDbContext.Stories.RemoveRange(fakeDbContext.Stories);
            await fakeDbContext.SaveChangesAsync();
        }
        private async void ClearAndLoadFakeStoriesToDb()
        {
            ClearDatabase();
            foreach (Story fakeStory in fakeStoryList)
            {
                fakeDbContext.Stories.Add(fakeStory);
                await fakeDbContext.SaveChangesAsync();
            }
        }

        #region GetStoriesAsync

        [Fact]
        public async void GetStoriesAsync_WithValidPageNumberAndPageSize_ReturnsStories()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Stories.AddRange(fakeStoryList);
            await fakeDbContext.SaveChangesAsync();

            var pageNumber = 1;
            var pageSize = 2;

            // Act
            var result = await fakeStoryRepository.GetStoriesAsync(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageSize, result.Count);
            Assert.Equal(fakeStoryList[1].Title, result[0].Title);
            Assert.Equal(fakeStoryList[0].Title, result[1].Title);
        }

        #endregion

        #region GetStoryByIdAsync

        [Fact]
        public async void GetStoryByIdAsync_WithExistingId_ReturnsStoryCorrectly()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Stories.Add(fakeStory);
            await fakeDbContext.SaveChangesAsync();
            
            // Act
            var story = await fakeStoryRepository.GetStoryByIdAsync(fakeStory.Id);
            
            // Assert
            Assert.Equal(fakeStory, story);
        }

        #endregion

        #region PostStoryAsync

        [Fact]
        public async void PostStoryAsync_WithValidStory_ReturnsNewStory()
        {
            // Arrange
            ClearDatabase();

            // Act
            var createdStory = await fakeStoryRepository.PostStoryAsync(fakeStory);

            // Assert
            Assert.NotNull(createdStory);
            Assert.NotEqual(0, createdStory.Id);
            Assert.Equal(fakeStory.Title, createdStory.Title);
        }

        #endregion

        #region PostStoryAsync

        [Fact]
        public async void UpdateStoryByIdAsync_WithExistingIdAndValidStory_ReturnsUpdatedStory()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Stories.Add(fakeStory);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeStoryRepository.UpdateStoryByIdAsync(fakeStory.Id, fakeUpdateStory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUpdateStory.Id, result.Id);
            Assert.Equal(fakeUpdateStory.Title, result.Title);
            Assert.Equal(fakeUpdateStory.Body, result.Body);
        }

        #endregion

        #region DeleteStoryByIdAsync

        [Fact]
        public async void DeleteStoryByIdAsync_WithExistingId_DeletesStoryAndReturnsTrue()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Stories.Add(fakeStory);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeStoryRepository.DeleteStoryByIdAsync(fakeStory.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(0, await fakeDbContext.Stories.CountAsync());
        }

        #endregion

        #region CountStoriesAsync

        [Fact]
        public async void CountStoriesAsync_WhenStoriesExist_ReturnsCorrectCount()
        {
            // Arrange
            ClearAndLoadFakeStoriesToDb();

            // Act
            var count = await fakeStoryRepository.CountStoriesAsync();

            // Assert
            Assert.Equal(fakeStoryList.Count, count);
        }

        [Fact]
        public async void CountStoriesAsync_WithNoStories_ReturnsZero()
        {
            // Arrange
            ClearDatabase();

            // Act
            var count = await fakeStoryRepository.CountStoriesAsync();

            // Assert
            Assert.Equal(0, count);
        }

        #endregion

    }
}
