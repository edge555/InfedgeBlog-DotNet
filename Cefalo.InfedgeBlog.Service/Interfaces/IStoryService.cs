using Cefalo.InfedgeBlog.Database.Model;
namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<List<Story>> GetStoriesAsync();
        Task<Story> GetStoryByIdAsync(int Id);
        Task<Story> PostStoryAsync(Story Story);
        Task<Story> UpdateStoryAsync(int Id, Story story);
    }
}
