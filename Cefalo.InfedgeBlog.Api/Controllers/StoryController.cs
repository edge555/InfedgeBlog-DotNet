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
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;

        public StoryController(IStoryService storyService, ILogger<StoryController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> GetStoriesAsync([FromQuery] PaginationFilter filter)
        {
            _logger.LogInformation("Retrieving stories");

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetStoriesAsync(validFilter.PageNumber, validFilter.PageSize);
            var pagedDataList = pagedData.ToList();
            var totalRecords = await _storyService.CountStoriesAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<StoryDto>(pagedDataList, validFilter, totalRecords);

            _logger.LogInformation("Retrieved stories successfully");
            return Ok(pagedReponse);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStoryByIdAsync(int Id)
        {
            _logger.LogInformation("Retrieving story with ID: {Id}", Id);

            var story = await _storyService.GetStoryByIdAsync(Id);
            if (story == null)
            {
                _logger.LogWarning("Story not found with ID: {Id}", Id);
                return BadRequest("Story not found.");
            }

            return Ok(story);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> PostStoryAsync([FromBody] StoryPostDto storyPostDto)
        {
            _logger.LogInformation("Creating a new story");

            var storyDto = await _storyService.PostStoryAsync(storyPostDto);
            if (storyDto == null)
            {
                _logger.LogWarning("Failed to create story");
                return BadRequest("Can not create story.");
            }

            _logger.LogInformation("Story created successfully");
            return Created("", storyDto);
        }

        [HttpPut("{Id}"), Authorize]
        public async Task<IActionResult> UpdateStoryByIdAsync(int Id, [FromBody] StoryUpdateDto storyUpdateDto)
        {
            _logger.LogInformation("Updating story with ID: {Id}", Id);

            var storyDto = await _storyService.UpdateStoryByIdAsync(Id, storyUpdateDto);
            if (storyDto == null)
            {
                _logger.LogWarning("Failed to update story with ID: {Id}", Id);
                return BadRequest("Can not update story.");
            }

            _logger.LogInformation("Story updated successfully");
            return Ok(storyDto);
        }

        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteStoryByIdAsync(int Id)
        {
            _logger.LogInformation("Deleting story with ID: {Id}", Id);

            var deleted = await _storyService.DeleteStoryByIdAsync(Id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete story with ID: {Id}", Id);
                return BadRequest("Can not delete story.");
            }

            _logger.LogInformation("Story deleted successfully");
            return NoContent();
        }
    }
}
