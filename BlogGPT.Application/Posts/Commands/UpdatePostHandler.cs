using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Domain.Exceptions;
using System.Text.Json;

namespace BlogGPT.Application.Posts.Commands
{
	public record UpdatePostCommand : IRequest<int>
	{
		public int Id { get; set; }

		public int[]? CategoryIds { get; set; }

		public required string Title { set; get; }

		public string? Thumbnail { set; get; }

		public string? Description { set; get; }

		public required string Content { set; get; }

		public required string RawText { set; get; }

		public bool IsPublished { set; get; }
	}
	public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, int>
	{
		private readonly IApplicationDbContext _context;
		private readonly IChatbot _chatbot;

		public UpdatePostHandler(IApplicationDbContext context, IChatbot chatbot)
		{
			_context = context;
			_chatbot = chatbot;
		}

		public async Task<int> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
		{
			var entity = await _context.Posts.Include(post => post.EmbeddingPost).FirstOrDefaultAsync(post => post.Id == command.Id, cancellationToken) ?? throw new NotFoundException(nameof(Post), command.Id);
			var oldSlug = entity.Slug;

			entity.Title = command.Title;
			entity.Thumbnail = command.Thumbnail;
			entity.Description = command.Description;
			entity.Content = command.Content;
			entity.IsPublished = command.IsPublished;
			entity.Slug = command.Title.GenerateSlug();

			if (oldSlug != entity.Slug)
			{
				var existedSlug = await _context.Posts
					.AnyAsync(post => post.Slug == entity.Slug, cancellationToken);

				if (existedSlug)
				{
					var latestPost = await _context.Posts
						.OrderByDescending(post => post.Id)
						.FirstOrDefaultAsync(cancellationToken);

					if (latestPost != null)
					{
						entity.Slug = $"{entity.Slug}-{latestPost.Id + 1}";
					}
					else
					{
						entity.Slug = $"{entity.Slug}-1";
					}
				};
			}

			var postCategories = await _context.PostCategories
				.Where(postCategories => postCategories.PostId == command.Id)
				.ToListAsync(cancellationToken);


			if (postCategories.Any())
			{
				var deletedPostCategories = command.CategoryIds != null
					? postCategories.Where(postCategory => !command.CategoryIds.Contains(postCategory.CategoryId))
					: postCategories;

				_context.PostCategories.RemoveRange(deletedPostCategories);
			}

			if (command.CategoryIds != null)
			{
				var existedCategoryIds = postCategories.Select(postCategory => postCategory.CategoryId);

				var addPostCategories = command.CategoryIds?
					.Where(categoryId => !existedCategoryIds.Contains(categoryId))
					.Select(categoryId => new PostCategory { CategoryId = categoryId, PostId = command.Id }).ToList();

				if (addPostCategories != null) _context.PostCategories.AddRange(addPostCategories);
			}

			if (command.IsPublished)
			{
				var chunkTexts = new List<string> { command.RawText, command.Title };
				chunkTexts.AddRange(command.RawText.Split("\n\n"));

				var embeddings = _chatbot.GetEmbeddings(chunkTexts);

				if (entity.EmbeddingPost != null)
				{
					_context.EmbeddingPosts.Remove(entity.EmbeddingPost);
				}

				var embeddingPost = new EmbeddingPost
				{
					Embedding = JsonSerializer.Serialize(embeddings[0]),
					EmbeddingChunks = embeddings.Skip(1).Select(embedding => new EmbeddingChunk { Embedding = JsonSerializer.Serialize(embedding) }).ToList()
				};
				entity.EmbeddingPost = embeddingPost;
				entity.RawText = entity.Title + "\n" + command.RawText;
			}

			_context.Posts.Update(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}