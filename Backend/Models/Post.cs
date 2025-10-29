namespace Backend.Models
{
    public class Post
    {
        public int RowNumber { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? Author { get; set; }
        public string? AuthorImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}