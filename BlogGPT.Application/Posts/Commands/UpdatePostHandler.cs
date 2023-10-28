using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Domain.Exceptions;
using System.Text.Json;

namespace BlogGPT.Application.Posts.Commands
{
    public record UpdatePostCommand : IRequest
    {
        public int Id { get; set; }

        public int[]? CategoryIds { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public required string RawText { set; get; }

        public bool IsPublished { set; get; }
    }
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IChatbot _chatbot;

        public UpdatePostHandler(IApplicationDbContext context, IChatbot chatbot)
        {
            _context = context;
            _chatbot = chatbot;
        }

        public async Task Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Posts.Include(post => post.EmbeddingPost).FirstOrDefaultAsync(post => post.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Post), command.Id);
            }

            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.Content = command.Content;
            entity.IsPublished = command.IsPublished;
            entity.Slug = command.Title.GenerateSlug();

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
                var contextValue = $"{command.Title}:\n{command.RawText}";
                var embedding = _chatbot.GetEmbedding(contextValue);

                if (entity.EmbeddingPost == null)
                {
                    var embeddingPost = new EmbeddingPost { Embedding = JsonSerializer.Serialize(embedding), RawText = contextValue };

                    entity.EmbeddingPost = embeddingPost;
                }
                else
                {
                    entity.EmbeddingPost.Embedding = JsonSerializer.Serialize(embedding);
                    entity.EmbeddingPost.RawText = contextValue;
                }
            }

            _context.Posts.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}