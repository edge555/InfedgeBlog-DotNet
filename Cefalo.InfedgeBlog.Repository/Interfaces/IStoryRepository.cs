using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Repository.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetStoriesAsync(int pageNumber, int pageSize);
        Task<Story> GetStoryByIdAsync(int Id);
        Task<Story> PostStoryAsync(Story story);
        Task<Story> UpdateStoryByIdAsync(int Id, Story story);
        Task<Boolean> DeleteStoryByIdAsync(int Id);
        Task<int> CountStoriesAsync();
    }
}
