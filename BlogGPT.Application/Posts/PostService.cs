
using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using System.Text.Json;

namespace BlogGPT.Application.Posts
{
	public class PostService(IApplicationDbContext context, IChatbot chatbot) : IPostService
	{
		private readonly IApplicationDbContext _context = context;
		private readonly IChatbot _chatbot = chatbot;

		public async Task<bool> ImportPostAsync(CancellationToken cancellationToken = default)
		{
			string importURL = @"imports/articles.txt";
			string storedPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"wwwroot/{importURL}"));

			using var sr = new StreamReader(storedPath);

			var text = sr.ReadToEnd();
			int slugIndex = 110;
			var posts = text.Split("\nArticle: ").Where(part => part.Length > 10).Select(part =>
			{
				var contentIndex = part.IndexOf("Content: ");
				var content = part[(contentIndex + 9)..].Replace("\r\n", "\n");
				var title = part[0..contentIndex];
				return new Post
				{
					Title = title,
					Slug = title.GenerateSlug() + $"-{slugIndex++}",
					Content = content,
					RawText = content,
					View = new View { Count = 0 },
				};
			}).ToList();

			if (posts.Count > 0)
			{
				await _context.Posts.AddRangeAsync(posts, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);
				return true;
			}
			return false;
		}

		public async Task<bool> DeleteEmbeddingPostAsync(CancellationToken cancellationToken = default)
		{
			var embeddings = await _context.EmbeddingPosts.ToListAsync(cancellationToken);

			if (embeddings.Count > 0)
			{
				_context.EmbeddingPosts.RemoveRange(embeddings);
				await _context.SaveChangesAsync(cancellationToken);
				return true;
			}
			return false;
		}

		public async Task<bool> EmbeddingPostAsync(CancellationToken cancellationToken = default)
		{
			var posts = await _context.Posts.Where(post => post.EmbeddingPost == null).ToListAsync(cancellationToken);

			int postCount = posts.Count;
			if (postCount > 0)
			{
				var chunkTextsList = posts.Select(post =>
				{
					var chunkTexts = new List<string> { post.RawText, post.Title };
					chunkTexts.AddRange(post.RawText.Split("\n\n").Where(chunk => chunk.Length > 10));
					return chunkTexts;
				}).ToList();


				var embeddingPosts = _chatbot.GetEmbeddingsList(chunkTextsList);

				for (var i = 0; i < postCount; i++)
				{
					posts[i].EmbeddingPost = new EmbeddingPost
					{
						Embedding = JsonSerializer.Serialize(embeddingPosts[i][0]),
						EmbeddingChunks = embeddingPosts[i].Skip(1).Select(embedding => new EmbeddingChunk { Embedding = JsonSerializer.Serialize(embedding) }).ToList()
					};
					posts[i].IsPublished = true;
				}

				_context.Posts.UpdateRange(posts);
				await _context.SaveChangesAsync(cancellationToken);
				return true;

			}
			return false;
		}
	}
}
