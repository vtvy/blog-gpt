using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using System.Numerics.Tensors;
using System.Text.Json;

namespace BlogGPT.Application.Chats
{
    public record ChatRequest
    {
        public required string Message { get; set; }
    }

    public class ChatService : IChatService
    {
        private IApplicationDbContext _context;
        private readonly IChatbot _chatbot;
        public ChatService(IChatbot chatbot, IApplicationDbContext context)
        {
            _context = context;
            _chatbot = chatbot;
        }

        public async IAsyncEnumerable<string> SendStreamingAsync(ChatRequest request)
        {
            var minRelevanceScore = 0.5;
            var limit = 1;

            var embeddingQuestion = _chatbot.GetEmbeddings([request.Message.Replace("?", "")])[0];

            List<RelevanceChunk> relevancePosts = [];

            await foreach (var relevanceChunk in _context.EmbeddingChunks
                .Select(embeddingChunk => new RelevanceChunk
                {
                    EmbeddingChunk = embeddingChunk,
                    EmbeddingPostId = embeddingChunk.EmbeddingPostId
                })
                .AsNoTracking()
                .AsAsyncEnumerable())
            {
                if (relevanceChunk != null)
                {
                    double similarityScore = TensorPrimitives.CosineSimilarity(
                        new ReadOnlyMemory<float>(embeddingQuestion).Span,
                        new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(relevanceChunk.EmbeddingChunk.Embedding)).Span);

                    if (similarityScore >= minRelevanceScore)
                    {
                        relevanceChunk.SimilarityScore = similarityScore;
                        relevancePosts.Add(relevanceChunk);
                    }
                }
            };

            var mostRelevancePost = relevancePosts
                .OrderByDescending(relevancePost => relevancePost.SimilarityScore)
                .Take(limit)
                .FirstOrDefault();

            if (mostRelevancePost != null)
            {
                var chatContext = await _context.EmbeddingPosts
                    .Where(embeddingPost => embeddingPost.Id == mostRelevancePost.EmbeddingPostId)
                    .Select(embeddingPost => new DocumentContext { RawText = embeddingPost.RawText, Title = embeddingPost.Post.Title, Slug = embeddingPost.Post.Slug })
                    .FirstOrDefaultAsync();

                if (chatContext == null)
                {
                    yield return "Sorry, I cannot find the relevance document base on your question.";
                }
                else
                {
                    await foreach (var output in _chatbot.GetAnswerAsync(request.Message, string.Join("\n\n", chatContext.RawText)))
                    {
                        yield return output;
                    };
                    yield return $"Answered based on <a href=\"/posts/{chatContext.Slug}.html\" target=\"_blank\">{chatContext.Title}</a>";
                }
            }
            else
            {
                yield return "Sorry, I cannot find the relevance document base on your question.";
            }
        }
    }

    public record RelevanceChunk
    {
        public int EmbeddingPostId { get; set; }
        public EmbeddingChunk EmbeddingChunk { get; set; } = null!;
        public double SimilarityScore { get; set; }
    }

    public record DocumentContext
    {
        public string RawText { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}