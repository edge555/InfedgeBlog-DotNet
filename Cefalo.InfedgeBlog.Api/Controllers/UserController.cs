using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.InfedgeBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            return Ok(await _userService.GetUsersAsync());
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> PostUserAsync([FromBody] User userPostDto)
        {
            var userDto = await _userService.PostUserAsync(userPostDto);
            return Created("", userDto);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUserByIdAsync(int Id, [FromBody] User userUpdateDto)
        {
            var userDto = await _userService.UpdateUserByIdAsync(Id, userUpdateDto);
            return Ok(userDto);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(int Id)
        {
            await _userService.DeleteUserByIdAsync(Id);
            return NoContent();
        }
    }
}
