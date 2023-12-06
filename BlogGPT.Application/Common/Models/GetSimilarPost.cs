namespace BlogGPT.Application.Common.Models
{
    public class GetSimilarPost
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        public required string Embedding { get; set; }

        public float Similarity { get; set; }
    }
}
