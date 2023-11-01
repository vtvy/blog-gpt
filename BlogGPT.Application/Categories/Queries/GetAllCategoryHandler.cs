using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetAllCategoryQuery : IRequest<IEnumerable<TreeItem<GetAllCategory>>>;

    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<TreeItem<GetAllCategory>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TreeItem<GetAllCategory>>> Handle(GetAllCategoryQuery query, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .ProjectTo<GetAllCategory>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = categories.GenerateChildren(c => c.Id, c => c.ParentId);
            return result;
        }
    }
}
