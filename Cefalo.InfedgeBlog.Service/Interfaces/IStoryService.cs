using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<List<Story>> GetStoriesAsync();
        Task<Story> GetStoryByIdAsync(int Id);
        Task<StoryDto> PostStoryAsync(Story Story);
        Task<StoryDto> UpdateStoryAsync(int Id, Story story);
    }
}
