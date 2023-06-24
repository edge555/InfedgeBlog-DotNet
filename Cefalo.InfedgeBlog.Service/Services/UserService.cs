using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Interfaces;
namespace Cefalo.InfedgeBlog.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<User> users = await _userRepository.GetUsersAsync();
            return users;
        }
        public async Task<User> GetUserByIdAsync(int Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            return user;
        }
        public async Task<User> GetUserByUsernameAsync(string Username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(Username);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }
        public async Task<User> PostUserAsync(User userPostDto)
        {
            var newUser = await _userRepository.PostUserAsync(userPostDto);
            return newUser;
        }
        public async Task<User> UpdateUserByIdAsync(int Id, User userUpdateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            var updatedUser = await _userRepository.UpdateUserByIdAsync(Id, userUpdateDto);
           
            return updatedUser;
        }
        public async Task<Boolean> DeleteUserByIdAsync(int Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            return await _userRepository.DeleteUserByIdAsync(Id);
        }
    }
}
