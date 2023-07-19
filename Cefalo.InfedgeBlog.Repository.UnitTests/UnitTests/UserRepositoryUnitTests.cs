using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Repository.Repositories;
using Cefalo.InfedgeBlog.Repository.UnitTests.FakeData;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cefalo.InfedgeBlog.Repository.UnitTests 
{
    public class UserRepositoryUnitTests
    {
        private readonly ApplicationDbContext fakeDbContext;
        private readonly IUserRepository fakeUserRepository;
        private readonly FakeUserData fakeUserData;
        private readonly User fakeUser, fakeUpdateUser;
        private readonly List<User> fakeUserList;
        public UserRepositoryUnitTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "fakeDbtaContext").Options;
            fakeDbContext = new ApplicationDbContext(options);
            fakeUserRepository = new UserRepository(fakeDbContext);
            fakeUserData = A.Fake<FakeUserData>();
            fakeUser = fakeUserData.fakeUser;
            fakeUserList = fakeUserData.fakeUserList;
            fakeUpdateUser = fakeUserData.fakeUpdateUser;
        }

        private async void ClearDatabase()
        {
            fakeDbContext.Users.RemoveRange(fakeDbContext.Users);
            await fakeDbContext.SaveChangesAsync();
        }

        private async void ClearAndLoadFakeUsersToDb()
        {
            ClearDatabase();
            foreach (User fakeUser in fakeUserList)
            {
                fakeDbContext.Users.Add(fakeUser);
                await fakeDbContext.SaveChangesAsync();
            }
        }

        #region GetUsersAsync

        [Fact]
        public async void GetUsersAsync_WithValidPageNumberAndPageSize_ReturnsUsers()
        {
            // Arrange
            ClearAndLoadFakeUsersToDb();

            var pageNumber = 1;
            var pageSize = 2;

            // Act
            var result = await fakeUserRepository.GetUsersAsync(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageSize, result.Count);
            Assert.Equal(fakeUserList[0].Username, result[0].Username);
            Assert.Equal(fakeUserList[1].Username, result[1].Username);
        }

        #endregion

        #region GetUserByIdAsync

        [Fact]
        public async void GetUserByIdAsync_WithExistingId_ReturnsUser()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.Add(fakeUser);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeUserRepository.GetUserByIdAsync(fakeUser.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser.Username, result.Username);
        }

        #endregion

        #region GetUserByUsernameAsync

        [Fact]
        public async void GetUserByUsernameAsync_ExistingUsername_ReturnsUser()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.Add(fakeUser);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeUserRepository.GetUserByUsernameAsync(fakeUser.Username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser.Username, result.Username);
        }

        #endregion

        #region GetUserByUsernameAsync

        [Fact]
        public async void GetUserByEmailAsync_ExistingValidEmail_ReturnsUser()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.Add(fakeUser);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeUserRepository.GetUserByEmailAsync(fakeUser.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser.Email, result.Email);
        }

        #endregion

        #region PostUserAsync

        [Fact]
        public async void PostUserAsync_WithValidUser_CreatesUserAndReturnsNewUser()
        {
            // Arrange
            ClearDatabase();

            // Act
            var result = await fakeUserRepository.PostUserAsync(fakeUser);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(fakeUser.Username, result.Username);
        }

        #endregion

        #region UpdateUserByIdAsync

        [Fact]
        public async void UpdateUserByIdAsync_WithExistsingIdAndValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.Add(fakeUser);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeUserRepository.UpdateUserByIdAsync(fakeUser.Id, fakeUpdateUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUser.Id, result.Id);
            Assert.Equal(fakeUpdateUser.Name, result.Name);
            Assert.Equal(fakeUpdateUser.Password, result.Password);
        }

        #endregion

        #region DeleteUserByIdAsync

        [Fact]
        public async void DeleteUserByIdAsync_WithExistingId_DeletesUserAndReturnsTrue()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.Add(fakeUser);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var result = await fakeUserRepository.DeleteUserByIdAsync(fakeUser.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(0, await fakeDbContext.Users.CountAsync());
        }

        [Fact]
        public async void CountUsersAsync_WhenUsersExists_ReturnsCorrectCount()
        {
            // Arrange
            ClearDatabase();
            fakeDbContext.Users.AddRange(fakeUserList);
            await fakeDbContext.SaveChangesAsync();

            // Act
            var count = await fakeUserRepository.CountUsersAsync();

            // Assert
            Assert.Equal(fakeUserList.Count, count);
        }

        [Fact]
        public async void CountUsersAsync_WithNoUsers_ReturnsZero()
        {
            // Arrange
            ClearDatabase();

            // Act
            var count = await fakeUserRepository.CountUsersAsync();

            // Assert
            Assert.Equal(0, count);
        }

        #endregion

    }
}
