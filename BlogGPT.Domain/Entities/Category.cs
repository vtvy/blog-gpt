namespace BlogGPT.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        public required string Slug { get; set; }

        public int? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category>? ChildrenCategories { get; set; }

        public ICollection<PostCategory>? PostCategories { get; set; }
    }
}
