using Cefalo.InfedgeBlog.Database.Model;
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
        private readonly IAuthService _authService;
        public StoryController(IStoryService storyService, IAuthService authService)
        {
            _storyService = storyService;
            _authService = authService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetStoriesAsync()
        {
            return Ok(await _storyService.GetStoriesAsync());
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStory(int Id)
        {
            var story = await _storyService.GetStoryByIdAsync(Id);
            return Ok(story);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> PostStoryAsync([FromBody] StoryPostDto storyPostDto)
        {
            var storyDto = await _storyService.PostStoryAsync(storyPostDto);
            return Created("", storyDto);
        }
        [HttpPut("{Id}"), Authorize]
        public async Task<IActionResult> UpdateStoryAsync(int Id, [FromBody] StoryUpdateDto storyUpdateDto)
        {
            var storyDto = await _storyService.UpdateStoryAsync(Id, storyUpdateDto);
            return Ok(storyDto);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteStoryByIdAsync(int Id)
        {
            await _storyService.DeleteStoryByIdAsync(Id);
            return NoContent();
        }
    }
}
