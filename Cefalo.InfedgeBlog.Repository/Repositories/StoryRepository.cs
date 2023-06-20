using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.InfedgeBlog.Repository.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public StoryRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<Story>> GetStoriesAsync()
        {
            return await _dbcontext.Stories.ToListAsync();
        }
        public async Task<Story> GetStoryByIdAsync(int Id)
        {
            var story = await _dbcontext.Stories.FindAsync(Id);
            return story;
        }
        public async Task<Story> PostStoryAsync(Story story)
        {
            _dbcontext.Stories.Add(story);
            await _dbcontext.SaveChangesAsync();
            return story;
        }
    }
}
