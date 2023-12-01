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

        public string? Thumbnail { set; get; }

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

            var existedSlug = await _context.Posts
                .AnyAsync(post => post.Slug == entity.Slug, cancellationToken);

            if (existedSlug) return -1;

            if (command.IsPublished)
            {

                var chunkTexts = new List<string> { command.RawText, command.Title };
                chunkTexts.AddRange(command.RawText.Split("\n\n").Where(chunk => chunk.Length > 10));

                var embeddings = _chatbot.GetEmbeddings(chunkTexts);

                var embeddingPost = new EmbeddingPost
                {
                    Embedding = JsonSerializer.Serialize(embeddings[0]),
                    RawText = command.Title + "\n" + command.RawText,
                    EmbeddingChunks = embeddings.Skip(1).Select(embedding => new EmbeddingChunk { Embedding = JsonSerializer.Serialize(embedding) }).ToList()
                };

                entity.EmbeddingPost = embeddingPost;
            }

            entity.View = new View { Count = 0 };
            _context.Posts.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            entity.AddDomainEvent(new PostCreatedEvent(entity));

            return entity.Id;
        }
    }
}