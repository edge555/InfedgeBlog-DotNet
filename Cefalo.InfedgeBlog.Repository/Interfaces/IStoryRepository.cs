using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Repository.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetStoriesAsync();
        Task<Story> GetStoryByIdAsync(int Id);
        Task<Story> PostStoryAsync(Story story);
        Task<Story> UpdateStoryAsync(int Id, Story story);
    }
}
