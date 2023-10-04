using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Queries.GetAllCategory
{
    public record GetAllCategoryQuery : IRequest<IEnumerable<GetAllCategoryVM>>;

    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<GetAllCategoryVM>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCategoryVM>> Handle(GetAllCategoryQuery query, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.Include(category => category.Parent)
                                    .ProjectTo<GetAllCategoryVM>(_mapper.ConfigurationProvider)
                                    .ToListAsync(cancellationToken);

            var returnCategories = categories.AsEnumerable().Where(category => category.Parent == null).ToList();

            var returnsCategories = categories.Where(category => category.Parent == null).AsEnumerable();

            var returnssCategories = categories.AsEnumerable().Where(category => category.Parent == null);

            var returnsssCategories = categories.AsQueryable().Where(category => category.Parent == null);

            return categories;
        }
    }
}
