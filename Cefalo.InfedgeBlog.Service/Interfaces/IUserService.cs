using Cefalo.InfedgeBlog.Database.Models;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int userId);
        public Task<User> GetUserByUsernameAsync(string username);
        Task<User> PostUserAsync(User userPostDto);
        public Task<User> UpdateUserByIdAsync(int userId, User userUpdateDto);
        public Task<Boolean> DeleteUserByIdAsync(int userId);
    }
}
