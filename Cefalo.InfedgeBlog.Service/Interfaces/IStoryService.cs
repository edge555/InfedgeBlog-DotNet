using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDto>> GetStoriesAsync(int pageNumber, int pageSize);
        Task<StoryDto> GetStoryByIdAsync(int Id);
        Task<StoryDto> PostStoryAsync(StoryPostDto storyPostDto);
        Task<StoryDto> UpdateStoryAsync(int Id, StoryUpdateDto storyUpdateDto);
        Task<Boolean> DeleteStoryByIdAsync(int Id);
        Task<int> CountStoriesAsync();
    }
}
