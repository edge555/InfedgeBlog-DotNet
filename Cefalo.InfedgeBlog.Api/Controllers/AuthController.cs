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
        private readonly IUserService _userService;
        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        [HttpPost]
        [Route("Signup")]
        public async Task<ActionResult<UserDto>> SignupAsync(SignupDto request)
        {
            var user = await _authService.SignupAsync(request);
            return Created(nameof(SignupAsync),user);
        }

    }
}
