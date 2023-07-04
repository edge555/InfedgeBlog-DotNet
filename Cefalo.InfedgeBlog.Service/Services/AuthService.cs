using AutoMapper;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Cefalo.InfedgeBlog.Service.Utils;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public AuthService(IUserRepository userRepository, IMapper mapper, IJwtTokenHandler jwtTokenHandler, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
            _httpContextAccessor = httpContextAccessor;
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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Password = hashedPassword;
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
        public async Task<UserWithTokenDto> LoginAsync(LoginDto request)
        {
            var userByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (userByUsername == null)
            {
                throw new BadRequestException("No user exists with this username");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, userByUsername.Password))
            {
                throw new BadRequestException("Password does not match");
            }
            var userData = _mapper.Map<UserWithTokenDto>(userByUsername);
            var token = _jwtTokenHandler.GenerateJwtToken(userByUsername);
            userData.Token = token;
            return userData;
        }
        public int GetLoggedInUserId()
        {
            var Id = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                Id = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return Id;
            }
            return Id;
        }
    }
}
