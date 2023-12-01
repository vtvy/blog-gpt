using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetPostQuery : IRequest<GetPost?>
    {
        public required int Id { get; set; }
    }

    public class GetPostHandler : IRequestHandler<GetPostQuery, GetPost?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPost?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var existedPost = await _context.Posts
                .Select(post => new GetPost
                {
                    Id = post.Id,
                    CategoryIds = post.PostCategories != null ? post.PostCategories.Select(postCate => postCate.CategoryId).ToArray() : null,
                    Title = post.Title,
                    Description = post.Description,
                    Thumbnail = post.Thumbnail ?? "/default.png",
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Slug = post.Slug,
                })
                .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

            return existedPost;
        }
    }
}