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
        public async Task<List<Story>> GetStoriesAsync()
        {
            return await _storyRepository.GetStoriesAsync();
        }
        public async Task<Story> GetStoryByIdAsync(int Id)
        {
            return await _storyRepository.GetStoryByIdAsync(Id);
        }

        public async Task<Story> PostStoryAsync(Story story)
        {
            Story newStory = await _storyRepository.PostStoryAsync(story);
            return newStory;
        }
        public async Task<StoryDto> UpdateStoryAsync(int Id, Story story)
        {
            var storyData = await _storyRepository.UpdateStoryAsync(Id, story);
            return storyDto;
        }
    }
}
