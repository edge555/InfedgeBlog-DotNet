using AutoMapper;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
namespace Cefalo.InfedgeBlog.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public UserService(IUserRepository userRepository, IMapper mapper, IAuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            List<User> users = await _userRepository.GetUsersAsync();
            IEnumerable<UserDto> userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDtos;
        }
        public async Task<UserDto> GetUserByIdAsync(int Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<UserDto> GetUserByUsernameAsync(string Username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(Username);
            if (user == null)
            {
                throw new NotFoundException("User not found with this username");
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<UserDto> PostUserAsync(UserPostDto userPostDto)
        {
            var userData = _mapper.Map<User>(userPostDto);
            var newUser = await _userRepository.PostUserAsync(userData);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<UserDto> UpdateUserByIdAsync(int Id, UserUpdateDto userUpdateDto)
        {
            var loggedInUserId = _authService.GetLoggedInUserId();
            if (loggedInUserId != Id)
            {
                throw new UnauthorizedException("You are not authorized to perform this action.");
            }
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            var userData = _mapper.Map<User>(userUpdateDto);
            var updatedUser = await _userRepository.UpdateUserByIdAsync(Id, userData);
            var userDto = _mapper.Map<UserDto>(updatedUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUserByIdAsync(int Id)
        {
            var loggedInUserId = _authService.GetLoggedInUserId();
            if (loggedInUserId != Id)
            {
                throw new UnauthorizedException("You are not authorized to perform this action.");
            }
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            return await _userRepository.DeleteUserByIdAsync(Id);
        }
    }
}
