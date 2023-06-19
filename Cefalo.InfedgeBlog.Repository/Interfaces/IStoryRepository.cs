using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Repository.Interfaces
{
    public interface IStoryRepository
    {
        Task<Story> PostStoryAsync(Story story);
    }
}
