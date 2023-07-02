using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Service.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Story> Stories { get; set; }
    }
}
