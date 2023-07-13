using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PasswordModifiedAt { get; set; }
        public ICollection<Story> Stories { get; set; }
    }
}

