using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetSelectCategoryQuery : IRequest<IList<GetSelectCategoryVM>>;

    public class GetSelectCategoryHandler : IRequestHandler<GetSelectCategoryQuery, IList<GetSelectCategoryVM>>
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
        public int Id { get; set; }

        public required string Name { get; set; }

        public IList<GetSelectCategoryVM>? ChildrenCategories { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Category, GetSelectCategoryVM>();
            }
        }
    }
}