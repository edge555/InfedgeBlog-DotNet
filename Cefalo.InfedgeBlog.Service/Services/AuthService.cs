using AutoMapper;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IDateTimeHandler _dateTimeHandler;
        private readonly IPasswordHandler _passwordHandler;
        private readonly DtoValidatorBase<SignupDto> _signupDtoValidator;
        private readonly DtoValidatorBase<LoginDto> _loginDtoValidator;
        public AuthService(IUserRepository userRepository, IMapper mapper, IJwtTokenHandler jwtTokenHandler, IDateTimeHandler dateTimeHandler, IPasswordHandler passwordHandler, DtoValidatorBase<SignupDto> signupDtoValidator, DtoValidatorBase<LoginDto> loginDtoValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
            _dateTimeHandler = dateTimeHandler;
            _passwordHandler = passwordHandler;
            _signupDtoValidator = signupDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }
        public async Task<UserDto> SignupAsync(SignupDto request)
        {
            _signupDtoValidator.ValidateDto(request);
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
            string hashedPassword = _passwordHandler.HashPassword(request.Password);
            user.Password = hashedPassword;
            user.CreatedAt = _dateTimeHandler.GetCurrentUtcTime();
            user.UpdatedAt = _dateTimeHandler.GetCurrentUtcTime();
            user.PasswordModifiedAt = _dateTimeHandler.GetCurrentUtcTime();
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
            _loginDtoValidator.ValidateDto(request);
            var userByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (userByUsername == null)
            {
                throw new BadRequestException("No user exists with this username");
            }
            if (!_passwordHandler.Verify(request.Password, userByUsername.Password))
            {
                throw new BadRequestException("Password does not match");
            }
            var userData = _mapper.Map<UserWithTokenDto>(userByUsername);
            var token = _jwtTokenHandler.GenerateJwtToken(userByUsername);
            userData.Token = token;
            return userData;
        }
        public void LogoutAsync()
        {
            _jwtTokenHandler.DeleteToken();
        }
    }
}
