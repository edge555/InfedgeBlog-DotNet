using AutoMapper;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> SignupAsync(SignupDto request)
        {
            var userByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (userByUsername != null)
            {
                throw new BadRequestException("User already exists with this username");
            }
            var userByEmail = await _userRepository.GetUserByEmailAsync(request.Email);
            if (userByEmail != null)
            {
                throw new BadRequestException("User already exists with this email");
            }
            var user = _mapper.Map<User>(request);
            var userData = 
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordModifiedAt = DateTime.UtcNow;
            var newUser = await _userRepository.PostUserAsync(user);
            if(newUser == null) 
            {
                throw new BadRequestException("Can not create user");
            }
            var userDto = _mapper.Map<UserDto>(newUser);
            return userDto;
        }
        public async Task<UserDto> LoginAsync(LoginDto request)
        {
            throw new NotImplementedException();
        }
    }
}
