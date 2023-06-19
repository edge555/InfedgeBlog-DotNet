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

        [HttpPost]
        public async Task<IActionResult> PostStoryAsync(Story story)
        {
            var storyDtoObj = await _storyService.PostStoryAsync(story);
            return Created("", storyDtoObj);
        }
    }
}
