using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Queries;

public class GetCategoryQuery : IRequest<GetCategoryVM?>
{
    public required int Id { get; set; }
}

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, GetCategoryVM?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategoryVM?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var existedCategory = await _context.Categories
            .ProjectTo<GetCategoryVM>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

        return existedCategory;
    }
}
public class GetCategoryVM
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Slug { get; set; }

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public string? Parent { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, GetCategoryVM>()
                .ForMember(destination => destination.Parent, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null));
        }
    }
}
