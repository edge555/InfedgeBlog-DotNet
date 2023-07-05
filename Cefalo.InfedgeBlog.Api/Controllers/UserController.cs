using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.InfedgeBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, IAuthService authService)
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
        [HttpPost, Authorize]
        public async Task<IActionResult> PostUserAsync([FromBody] UserPostDto userPostDto)
        {
            var userDto = await _userService.PostUserAsync(userPostDto);
            if(userDto== null) 
            {
                return BadRequest("Can not create user");
            }
            return Created("", userDto);
        }
        [HttpPut("{Id}"), Authorize]
        public async Task<IActionResult> UpdateUserByIdAsync(int Id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var userDto = await _userService.UpdateUserByIdAsync(Id, userUpdateDto);
            if (userDto == null)
            {
                return BadRequest("Can not update user");
            }
            return Ok(userDto);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteUserByIdAsync(int Id)
        {
            var deleted = await _userService.DeleteUserByIdAsync(Id);
            if (!deleted) 
            { 
                return BadRequest("Can not delete user"); 
            }
            return NoContent();
        }
    }
}
