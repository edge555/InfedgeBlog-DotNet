using Cefalo.InfedgeBlog.Database.Model;

namespace Cefalo.InfedgeBlog.Service.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime JoinedDate { get; set; }
        public ICollection<Story> Stories { get; set; }
    }
}
