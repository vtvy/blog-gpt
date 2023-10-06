using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Queries;

public class GetCategoryQuery : IRequest<GetCategoryVM>
{
    public required int Id { get; set; }
}

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, GetCategoryVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategoryVM> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var existedCategory = await _context.Categories.Include(category => category.Parent)
                    .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

        var returnCategory = _mapper.Map<GetCategoryVM>(existedCategory);

        return returnCategory;
    }
}
public class GetCategoryVM
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public GetCategoryVM? Parent { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, GetCategoryVM>();
        }
    }
}
