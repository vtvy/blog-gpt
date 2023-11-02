namespace BlogGPT.Application.Common.Models
{
    public class GetPost
    {
        public int Id { get; set; }

        public int[]? CategoryIds { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Content { get; set; }

        public bool IsPublished { get; set; }

        public required string Slug { get; set; }
    }
}