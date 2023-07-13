using Cefalo.InfedgeBlog.Database.Models;

namespace Cefalo.InfedgeBlog.Database.Model
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }  
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

