using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Events;

namespace BlogGPT.Application.Posts.Commands
{
    public record CreatePostCommand : IRequest<int>
    {
        public int[]? CategoryIds { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public bool Published { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<CreatePostCommand, Post>()
                    .ForMember(destination => destination.Slug,
                                opt => opt.MapFrom(src => src.Title.GenerateSlug()))
                    .ForMember(destination => destination.PostCategories,
                                opt => opt.MapFrom(src => src.CategoryIds != null
                                ? src.CategoryIds.Select(cateId => new PostCategory { CategoryId = cateId })
                                : null));
            }
        }
    }
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Post>(command);

            _context.Posts.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            entity.AddDomainEvent(new PostCreatedEvent(entity));

            return entity.Id;
        }
    }
}