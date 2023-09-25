using BlogGPT.Domain.Common;

namespace BlogGPT.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string AuthorId { get; set; } = default!;
        public int? ParentId { get; set; }
        public User Author { get; set; } = default!;
        public Category? Parent {  get; set; }
        public ICollection<Category>? ChildrenCategories { get; set; }
        public ICollection<ArticleCategory>? ArticleCategories { get; set; }
    }
}
