using BlogGPT.Domain.Common;

namespace BlogGPT.Domain.Entities
{
    public class Article : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Image? Thumbnail { get; set; }
        public string Content { get; set; } = default!;
        public bool IsPublished { get; set; }
        public string AuthorId { get; set; } = default!;
        public User Author { get; set; } = default!;
        public ICollection<ArticleCategory>? ArticleCategories { get; set; }
    }
}
