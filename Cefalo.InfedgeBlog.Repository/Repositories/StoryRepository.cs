using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;

namespace Cefalo.InfedgeBlog.Repository.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public StoryRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<Story> PostStoryAsync(Story story)
        {
            _dbcontext.Stories.Add(story);
            await _dbcontext.SaveChangesAsync();
            return story;
        }
    }
}
