using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.InfedgeBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Signup")]
        public async Task<ActionResult<UserDto>> SignupAsync(SignupDto request)
        {
            _logger.LogInformation("Signing up user");

            var userDto = await _authService.SignupAsync(request);
            if (userDto == null)
            {
                _logger.LogWarning("Failed to sign up user");
                return BadRequest("Can not signup.");
            }

            _logger.LogInformation("User signed up successfully");
            return Created(nameof(SignupAsync), userDto);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserWithTokenDto>> LoginAsync(LoginDto request)
        {
            _logger.LogInformation("Logging in user");

            var userWithToken = await _authService.LoginAsync(request);
            if (userWithToken == null)
            {
                _logger.LogWarning("Failed to login user");
                return BadRequest("Can not login.");
            }

            _logger.LogInformation("User logged in successfully");
            return Ok(userWithToken);
        }
    }
}
