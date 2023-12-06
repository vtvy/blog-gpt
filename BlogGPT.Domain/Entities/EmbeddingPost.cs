namespace BlogGPT.Domain.Entities
{
	public class EmbeddingPost
	{
		public int Id { get; set; }
		public required string Embedding { get; set; }

		public int PostId { get; set; }

		public Post Post { get; set; } = null!;

		public ICollection<EmbeddingChunk>? EmbeddingChunks { get; set; }
	}
}