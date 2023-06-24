using Cefalo.InfedgeBlog.Database.Model;
using System.ComponentModel.DataAnnotations;

namespace Cefalo.InfedgeBlog.Database.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public ICollection<Story> Stories { get; set; }
    }
}
