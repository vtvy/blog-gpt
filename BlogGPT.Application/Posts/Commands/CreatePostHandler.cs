using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using BlogGPT.Domain.Events;
using System.Text.Json;

namespace BlogGPT.Application.Posts.Commands
{
    public record CreatePostCommand : IRequest<int>
    {
        public int[]? CategoryIds { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public required string RawText { set; get; }

        public bool IsPublished { set; get; }

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
        private readonly IChatbot _chatbot;

        public CreatePostHandler(IApplicationDbContext context, IMapper mapper, IChatbot chatbot)
        {
            _context = context;
            _mapper = mapper;
            _chatbot = chatbot;
        }

        public async Task<int> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Post>(command);

            if (command.IsPublished)
            {
                var contextValue = $"{command.Title}:\n{command.RawText}";
                var embedding = _chatbot.GetEmbedding(contextValue);

                var embeddingPost = new EmbeddingPost { Embedding = JsonSerializer.Serialize(embedding), RawText = contextValue };

                entity.EmbeddingPost = embeddingPost;
            }

            _context.Posts.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            entity.AddDomainEvent(new PostCreatedEvent(entity));

            return entity.Id;
        }
    }
}