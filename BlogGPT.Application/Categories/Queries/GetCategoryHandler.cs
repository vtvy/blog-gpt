using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Categories.Queries;

public record GetCategoryQuery : IRequest<GetCategory?>
{
    public required int Id { get; set; }
}

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, GetCategory?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategory?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var existedCategory = await _context.Categories
            .ProjectTo<GetCategory>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

        return existedCategory;
    }
}