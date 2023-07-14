using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
        public Task<UserDto> GetUserByIdAsync(int userId);
        public Task<UserDto> GetUserByUsernameAsync(string username);
        Task<UserDto> PostUserAsync(UserPostDto userPostDto);
        public Task<UserDto> UpdateUserByIdAsync(int userId, UserUpdateDto userUpdateDto);
        public Task<Boolean> DeleteUserByIdAsync(int userId);
        Task<int> CountUsersAsync();
    }
}
