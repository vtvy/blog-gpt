namespace BlogGPT.Domain.Entities
{
    public class Category : BaseEntity, ITree<Category>
    {
        public required string Name { get; set; }

        public required string Slug { get; set; }

        public int? ParentId { get; set; }

        public string? Description { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category>? Children { get; set; }

        public ICollection<PostCategory>? PostCategories { get; set; }
    }
}
