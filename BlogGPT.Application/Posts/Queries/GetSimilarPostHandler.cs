using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;
using System.Numerics.Tensors;
using System.Text.Json;

namespace BlogGPT.Application.Posts.Queries
{
    public record GetSimilarPostQuery : IRequest<IEnumerable<GetSimilarPost>>
    {
        public int Id { get; set; }
    }

    public class GetSimilarPostHandler(IApplicationDbContext context) : IRequestHandler<GetSimilarPostQuery, IEnumerable<GetSimilarPost>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<IEnumerable<GetSimilarPost>> Handle(GetSimilarPostQuery request, CancellationToken cancellationToken)
        {
            var existedPost = await _context.EmbeddingPosts.Where(e => e.PostId == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (existedPost == null)
            {
                return new List<GetSimilarPost>();
            }
            else
            {
                var similarPosts = new List<GetSimilarPost>();

                float threshold = 0.4f;

                await foreach (var similarPost in _context.EmbeddingPosts.Select(e => new GetSimilarPost
                {
                    Title = e.Post.Title,
                    Slug = e.Post.Slug,
                    Description = e.Post.Description != null ? string.Join("", e.Post.Description.Take(50)) + "..." : null,
                    Embedding = e.Embedding,
                    Id = e.PostId

                }).AsAsyncEnumerable())
                {
                    if (similarPost.Id == request.Id)
                    {
                        continue;
                    }
                    var similarity = TensorPrimitives.CosineSimilarity(
                        new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(existedPost.Embedding)).Span,
                        new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(similarPost.Embedding)).Span
                    );

                    if (similarity >= threshold)
                    {
                        similarPost.Similarity = similarity;
                        similarPosts.Add(similarPost);
                    }
                }
                return similarPosts.OrderByDescending(s => s.Similarity).Take(5);
            }
        }
    }
}