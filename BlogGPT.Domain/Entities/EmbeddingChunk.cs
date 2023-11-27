namespace BlogGPT.Domain.Entities
{
    public class EmbeddingChunk
    {
        public int Id { get; set; }

        public int EmbeddingPostId { get; set; }

        public required string Embedding { get; set; }

        public EmbeddingPost EmbeddingPost { get; set; } = null!;
    }
}