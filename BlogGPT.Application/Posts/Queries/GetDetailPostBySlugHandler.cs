using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetDetailPostBySlugQuery : IRequest<GetDetailPost?>
    {
        public required string Slug { get; set; }
    }

    public class GetDetailPostBySlugHandler : IRequestHandler<GetDetailPostBySlugQuery, GetDetailPost?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTimeService;

        public GetDetailPostBySlugHandler(IApplicationDbContext context, IDateTime dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public async Task<GetDetailPost?> Handle(GetDetailPostBySlugQuery request, CancellationToken cancellationToken)
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
                    Thumbnail = post.Thumbnail ?? "/default.png",
                    LastModifiedAt = post.LastModifiedAt,
                    LastModifiedBy = post.LastModifiedBy,
                    CreatedBy = post.Author != null ? post.Author.NormalizedUserName : null,
                    View = post.View != null ? post.View.Count : 0,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(post => post.Slug == request.Slug, cancellationToken);

            if (post != null)
            {
                var view = await _context.Views.FirstOrDefaultAsync(view => view.PostId == post.Id, cancellationToken);
                if (view == null)
                {
                    _context.Views.Add(new View
                    {
                        PostId = post.Id,
                        Count = 1,
                    });
                }
                else
                {
                    view.Count += 1;
                    _context.Views.Update(view);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }
    }
}