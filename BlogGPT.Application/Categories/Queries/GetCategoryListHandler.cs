using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetCategoryListQuery : IRequest<IEnumerable<GetAllCategory>>;

    public class GetCategoryListHandler : IRequestHandler<GetCategoryListQuery, IEnumerable<GetAllCategory>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryListHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCategory>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .ProjectTo<GetAllCategory>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
