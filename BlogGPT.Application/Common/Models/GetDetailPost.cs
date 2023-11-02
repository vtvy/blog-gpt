namespace BlogGPT.Application.Common.Models
{
    public class GetDetailPost
    {
        public int Id { get; set; }

        public IEnumerable<GetAllCategory>? Categories { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public string? Thumbnail { get; set; }

        public required string Content { get; set; }

        public bool IsPublished { get; set; }

        public required string Slug { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public string? CreatedBy { get; set; }
    }
}