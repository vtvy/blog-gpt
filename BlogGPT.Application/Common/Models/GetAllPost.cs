namespace BlogGPT.Application.Common.Models
{
    public class GetAllPost
    {
        public int Id { get; set; }

        public IEnumerable<GetAllCategory>? Categories { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; }

        public string? Thumbnail { get; set; }

        public required string Slug { get; set; }
    }
}