using Cefalo.InfedgeBlog.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.InfedgeBlog.Database.Context
{
    public  class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Story> Stories { get; set; }
    }
}
