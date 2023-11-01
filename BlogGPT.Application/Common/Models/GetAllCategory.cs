namespace BlogGPT.Application.Common.Models
{
    public class GetAllCategory
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public int? ParentId { get; set; }
    }
}