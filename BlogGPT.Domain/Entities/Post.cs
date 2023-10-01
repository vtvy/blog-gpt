namespace BlogGPT.Domain.Entities
{
    public class Post : BaseAuditableEntity
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public string Slug { get; set; } = null!;

        public Image? Thumbnail { get; set; }

        public required string Content { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<PostCategory>? PostCategories { get; set; }
    }
}
