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
        [Required]
        public int AuthorId { get; set; }  
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

