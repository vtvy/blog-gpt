namespace BlogGPT.Application.Posts
{
	public interface IPostService
	{
		Task<bool> ImportPostAsync(CancellationToken cancellationToken = default);
		Task<bool> DeleteEmbeddingPostAsync(CancellationToken cancellationToken = default);
		Task<bool> EmbeddingPostAsync(CancellationToken cancellationToken = default);
	}
}
