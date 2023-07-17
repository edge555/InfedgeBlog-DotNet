using AutoMapper;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IDateTimeHandler _dateTimeHandler;
        private readonly DtoValidatorBase<UserPostDto> _userPostDtoValidator;
        private readonly DtoValidatorBase<UserUpdateDto> _userUpdateDtoValidator;
        public UserService(IUserRepository userRepository, IMapper mapper, IJwtTokenHandler jwtTokenHandler, IDateTimeHandler dateTimeHandler, DtoValidatorBase<UserPostDto> userPostDtoValidator, DtoValidatorBase<UserUpdateDto> userUpdateDtoValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
            _dateTimeHandler = dateTimeHandler;
            _userPostDtoValidator = userPostDtoValidator;
            _userUpdateDtoValidator = userUpdateDtoValidator;
        }
        public async Task<IEnumerable<UserDto>> GetUsersAsync(int pageNumber, int pageSize)
        {
            List<User> users = await _userRepository.GetUsersAsync(pageNumber, pageSize);
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
            _userPostDtoValidator.ValidateDto(userPostDto);
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            var userData = _mapper.Map<User>(userPostDto);
            userData.CreatedAt = _dateTimeHandler.GetCurrentUtcTime();
            userData.UpdatedAt = _dateTimeHandler.GetCurrentUtcTime();
            userData.PasswordModifiedAt = _dateTimeHandler.GetCurrentUtcTime();
            var newUser = await _userRepository.PostUserAsync(userData);
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<UserDto> UpdateUserByIdAsync(int Id, UserUpdateDto userUpdateDto)
        {
            _userUpdateDtoValidator.ValidateDto(userUpdateDto);
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            var loggedInUserId = _jwtTokenHandler.GetLoggedInUserId();
            if (loggedInUserId != Id)
            {
                throw new ForbiddenException("You do not have permission to perform this action.");
            }
            var userData = _mapper.Map<User>(userUpdateDto);
            userData.UpdatedAt = _dateTimeHandler.GetCurrentUtcTime();
            var updatedUser = await _userRepository.UpdateUserByIdAsync(Id, userData);
            var userDto = _mapper.Map<UserDto>(updatedUser);
            return userDto;
        }
        public async Task<Boolean> DeleteUserByIdAsync(int Id)
        {
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
            {
                throw new NotFoundException("No user found with this id");
            }
            var loggedInUserId = _jwtTokenHandler.GetLoggedInUserId();
            if (loggedInUserId != Id)
            {
                throw new ForbiddenException("You do not have permission to perform this action.");
            }
            return await _userRepository.DeleteUserByIdAsync(Id);
        }

        public async Task<int> CountUsersAsync()
        {
            return await _userRepository.CountUsersAsync();
        }
    }
}
