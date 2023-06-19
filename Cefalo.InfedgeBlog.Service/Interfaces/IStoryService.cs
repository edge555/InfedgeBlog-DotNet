using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IStoryService
    {
        Task<StoryDto> PostStoryAsync(Story Story);
    }
}
