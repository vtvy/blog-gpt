using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetDetailPostQuery : IRequest<GetDetailPost?>
    {
        public int Id { get; set; }
    }


    public class GetDetailPostHandler : IRequestHandler<GetDetailPostQuery, GetDetailPost?>
    {
        private readonly IApplicationDbContext _context;

        public GetDetailPostHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetDetailPost?> Handle(GetDetailPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts
                .Select(post => new GetDetailPost
                {
                    Id = post.Id,
                    Categories = post.PostCategories != null ? post.PostCategories.Select(postCate => new GetAllCategory
                    {
                        Id = postCate.Category.Id,
                        Name = postCate.Category.Name,
                        Slug = postCate.Category.Slug,
                    }) : null,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Slug = post.Slug,
                    Thumbnail = post.Thumbnail != null ? post.Thumbnail.Url : null,
                    LastModifiedAt = post.LastModifiedAt,
                    LastModifiedBy = post.LastModifiedBy,
                    CreatedBy = post.Author != null ? post.Author.NormalizedUserName : null,
                })
                .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

            return post;
        }
    }
}