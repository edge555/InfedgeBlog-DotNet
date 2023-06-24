using Cefalo.InfedgeBlog.Database.Models;

namespace Cefalo.InfedgeBlog.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int Id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> PostUserAsync(User user);
        Task<User> UpdateUserByIdAsync(int Id, User user);
        Task<Boolean> DeleteUserByIdAsync(int Id);
    }
}
