using Cefalo.InfedgeBlog.Database.Model;
namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<Story> PostStoryAsync(Story Story);
    }
}
