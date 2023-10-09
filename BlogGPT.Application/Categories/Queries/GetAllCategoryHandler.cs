using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetAllCategoryQuery : IRequest<IEnumerable<TreeItem<GetAllCategoryVM>>>;

    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<TreeItem<GetAllCategoryVM>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TreeItem<GetAllCategoryVM>>> Handle(GetAllCategoryQuery query, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .Select(category => new GetAllCategoryVM
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug,
                    ParentId = category.ParentId
                })
                .ToListAsync(cancellationToken);


            var result = categories.GenerateChildren(c => c.Id, c => c.ParentId);
            return result;
        }
    }
    public class GetAllCategoryVM
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public int? ParentId { get; set; }
    }
}
