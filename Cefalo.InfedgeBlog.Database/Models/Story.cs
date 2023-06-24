using Cefalo.InfedgeBlog.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace Cefalo.InfedgeBlog.Database.Model
{
    public class Story
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }  
        public User User { get; set; }
    }
}
