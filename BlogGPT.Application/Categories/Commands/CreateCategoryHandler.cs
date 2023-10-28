using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Categories.Commands
{
    public record CreateCategoryCommand : IRequest<int>
    {
        public int? ParentId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }


        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<CreateCategoryCommand, Category>()
                    .ForMember(destination => destination.Slug,
                        opt => opt.MapFrom(src => src.Name.GenerateSlug()));
            }
        }
    }

    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Category>(command);

            _context.Categories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
