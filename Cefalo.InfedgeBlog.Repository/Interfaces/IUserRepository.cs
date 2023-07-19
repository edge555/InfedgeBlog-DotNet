using Cefalo.InfedgeBlog.Database.Models;

namespace Cefalo.InfedgeBlog.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(int pageNumber, int pageSize);
        Task<User> GetUserByIdAsync(int Id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> PostUserAsync(User user);
        Task<User> UpdateUserByIdAsync(int Id, User user);
        Task<Boolean> DeleteUserByIdAsync(int Id);
        Task<int> CountUsersAsync();
    }
}
