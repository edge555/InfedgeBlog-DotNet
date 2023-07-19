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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync([FromQuery] PaginationFilter filter)
        {
            _logger.LogInformation("Retrieving users");

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _userService.GetUsersAsync(validFilter.PageNumber, validFilter.PageSize);
            var pagedDataList = pagedData.ToList();
            var totalRecords = await _userService.CountUsersAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<UserDto>(pagedDataList, validFilter, totalRecords);

            _logger.LogInformation("Retrieved users successfully");
            return Ok(pagedReponse);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserByIdAsync(int Id)
        {
            _logger.LogInformation("Retrieving user with ID: {Id}", Id);

            var userDto = await _userService.GetUserByIdAsync(Id);
            if (userDto == null)
            {
                _logger.LogWarning("User not found with ID: {Id}", Id);
                return BadRequest("User not found.");
            }

            return Ok(userDto);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> PostUserAsync([FromBody] UserPostDto userPostDto)
        {
            _logger.LogInformation("Creating a new user");

            var userDto = await _userService.PostUserAsync(userPostDto);
            if (userDto == null)
            {
                _logger.LogWarning("Failed to create user");
                return BadRequest("Can not create user.");
            }

            _logger.LogInformation("User created successfully");
            return Created("", userDto);
        }

        [HttpPut("{Id}"), Authorize]
        public async Task<IActionResult> UpdateUserByIdAsync(int Id, [FromBody] UserUpdateDto userUpdateDto)
        {
            _logger.LogInformation("Updating user with ID: {Id}", Id);

            var userDto = await _userService.UpdateUserByIdAsync(Id, userUpdateDto);
            if (userDto == null)
            {
                _logger.LogWarning("Failed to update user with ID: {Id}", Id);
                return BadRequest("Can not update user.");
            }

            _logger.LogInformation("User updated successfully");
            return Ok(userDto);
        }

        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteUserByIdAsync(int Id)
        {
            _logger.LogInformation("Deleting user with ID: {Id}", Id);

            var deleted = await _userService.DeleteUserByIdAsync(Id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete user with ID: {Id}", Id);
                return BadRequest("Can not delete user.");
            }

            _logger.LogInformation("User deleted successfully");
            return NoContent();
        }
    }
}
