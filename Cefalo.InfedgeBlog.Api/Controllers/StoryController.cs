using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.InfedgeBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
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
            if (story == null) { 
                return BadRequest("Story not found"); 
            }
            return Ok(story);
        }
        [HttpPost]
        public async Task<IActionResult> PostStoryAsync([FromBody] Story story)
        {
            var storyDtoObj = await _storyService.PostStoryAsync(story);
            return Created("", storyDtoObj);
        }
    }
}
