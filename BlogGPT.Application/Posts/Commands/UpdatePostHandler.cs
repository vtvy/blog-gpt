using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Exceptions;

namespace BlogGPT.Application.Posts.Commands
{
    public record UpdatePostCommand : IRequest
    {
        public int Id { get; set; }

        public int[]? CategoryIds { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public bool IsPublished { set; get; }
    }
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePostHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Posts.FindAsync(command.Id);

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

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}