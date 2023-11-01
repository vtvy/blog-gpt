using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetAllPostQuery : IRequest<PaginatedList<GetAllPost>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllPostHandler : IRequestHandler<GetAllPostQuery, PaginatedList<GetAllPost>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPostHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<GetAllPost>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.Posts.CountAsync();

            var pagingList = new PaginatedList<GetAllPost>(request.PageNumber, request.PageSize, count);

            var pagingPosts = await _context.Posts
                .OrderByDescending(post => post.LastModifiedAt)
                .Select(post => new GetAllPost
                {
                    Id = post.Id,
                    Categories = post.PostCategories != null
                    ? post.PostCategories.Select(postCate => new GetAllCategory
                    {
                        Id = postCate.Category.Id,
                        Name = postCate.Category.Name,
                        Slug = postCate.Category.Slug
                    }) : null,
                    Title = post.Title,
                    Description = post.Description,
                    Thumbnail = post.Thumbnail != null ? post.Thumbnail.Url : null,
                    IsPublished = post.IsPublished,
                    Slug = post.Slug,
                })
                .Skip((pagingList.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            pagingList.Items = pagingPosts;

            return pagingList;
        }
    }
}