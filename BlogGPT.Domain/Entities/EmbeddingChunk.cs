namespace BlogGPT.Domain.Entities
{
    public class EmbeddingChunk
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Embedding { get; set; }

        public Post Post { get; set; } = null!;
    }
}