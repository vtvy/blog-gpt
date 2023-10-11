using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetPostQuery : IRequest<GetPostVM?>
    {
        public required int Id { get; set; }
    }

    public class GetPostHandler : IRequestHandler<GetPostQuery, GetPostVM?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPostVM?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var existedPost = await _context.Posts
                .Select(post => new GetPostVM
                {
                    Id = post.Id,
                    CategoryIds = post.PostCategories != null ? post.PostCategories.Select(postCate => postCate.CategoryId).ToArray() : null,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Slug = post.Slug,
                })
                .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

            return existedPost;
        }
    }

    public class GetPostVM
    {
        public int Id { get; set; }

        public int[]? CategoryIds { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Content { get; set; }

        public bool IsPublished { get; set; }

        public required string Slug { get; set; }
    }
}