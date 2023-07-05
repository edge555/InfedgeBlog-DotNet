namespace Cefalo.InfedgeBlog.Service.Dtos
{
    public class StoryDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
