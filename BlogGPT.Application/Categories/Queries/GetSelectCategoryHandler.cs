using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetSelectCategoryQuery : IRequest<IList<GetSelectCategoryVM>>;

    public class GetSelectCategoryHandler
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSelectCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<GetSelectCategoryVM>> Handle(GetSelectCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.Include(category => category.ChildrenCategories)
                .ProjectTo<GetSelectCategoryVM>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var returnCategories = categories.AsEnumerable().ToList(); ;

            return returnCategories;
        }
    }
    public class GetSelectCategoryVM
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public IList<GetCategoryVM>? ChildrenCategories { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Category, GetSelectCategoryVM>();
            }
        }
    }
}