using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDto>> GetStoriesAsync();
        Task<StoryDto> GetStoryByIdAsync(int Id);
        Task<StoryDto> PostStoryAsync(StoryPostDto storyPostDto);
        Task<StoryDto> UpdateStoryAsync(int Id, StoryUpdateDto storyUpdateDto);
        Task<Boolean> DeleteStoryByIdAsync(int Id);

    }
}
