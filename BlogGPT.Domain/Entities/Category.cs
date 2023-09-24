namespace BlogGPT.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public int? ParentCategoryId { get; set; }
        public ICollection<Category>? ChildrenCategories { get; set; }

    }
}
