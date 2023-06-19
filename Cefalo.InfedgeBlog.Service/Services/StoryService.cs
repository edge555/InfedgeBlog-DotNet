using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }
            
        public async Task<StoryDto> PostStoryAsync(Story story)
        {
        Story newStory = await _storyRepository.PostStoryAsync(story);
            var storyDto = new StoryDto
            {
                Id = newStory.Id,
                Title = newStory.Title,
                Body = newStory.Body,
                PublishedDate = newStory.PublishedDate
            };
            return storyDto;
        }

    }
}
