using Cefalo.InfedgeBlog.Api.Utils.Pagination.Filter;
using Cefalo.InfedgeBlog.Api.Utils.Pagination.Helpers;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var pagedData = await _userService.GetUsersAsync(validFilter.PageNumber, validFilter.PageSize);
            var pagedDataList = pagedData.ToList();
            var totalRecords = await _userService.CountUsersAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<UserDto>(pagedDataList, validFilter, totalRecords);
            return Ok(pagedReponse);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserByIdAsync(int Id)
        {
            var userDto = await _userService.GetUserByIdAsync(Id);
            if (userDto == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(userDto);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> PostUserAsync([FromBody] UserPostDto userPostDto)
        {
            var userDto = await _userService.PostUserAsync(userPostDto);
            if(userDto== null) 
            {
                return BadRequest("Can not create user.");
            }
            return Created("", userDto);
        }
        [HttpPut("{Id}"), Authorize]
        public async Task<IActionResult> UpdateUserByIdAsync(int Id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var userDto = await _userService.UpdateUserByIdAsync(Id, userUpdateDto);
            if (userDto == null)
            {
                return BadRequest("Can not update user.");
            }
            return Ok(userDto);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteUserByIdAsync(int Id)
        {
            var deleted = await _userService.DeleteUserByIdAsync(Id);
            if (!deleted) 
            { 
                return BadRequest("Can not delete user."); 
            }
            return NoContent();
        }
    }
}
