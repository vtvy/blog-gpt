namespace BlogGPT.Application.Common.Models
{
    public class GetSelectCategory
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int? ParentId { get; set; }

        public string? Slug { get; set; }
    }
}