using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
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
        private readonly double _minRelevanceScore;
        private readonly int _limit;
        public ChatService(IChatbot chatbot, IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _chatbot = chatbot;
            var configString = configuration.GetSection("Chatbot:MinRelevanceScore").Value;
            _minRelevanceScore = configString != null ? double.Parse(configString) : 0.1;
            var limit = configuration.GetSection("Chatbot:Limit").Value;
            _limit = limit != null ? int.Parse(limit) : 100;
        }

        public async IAsyncEnumerable<string> SendStreamingAsync(ChatRequest request)
        {
            var embeddingQuestion = _chatbot.GetEmbedding(request.Message);

            List<RelevancePost> relevancePosts = new();

            await foreach (var embeddingPost in _context.EmbeddingPosts
                .AsNoTracking()
                .AsAsyncEnumerable())
            {
                if (embeddingPost != null)
                {
                    double similarityScore = TensorPrimitives.CosineSimilarity(new ReadOnlyMemory<float>(embeddingQuestion).Span, new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(embeddingPost.Embedding)).Span);

                    if (similarityScore >= _minRelevanceScore)
                    {
                        relevancePosts.Add(new RelevancePost { EmbeddingPost = embeddingPost, SimilarityScore = similarityScore });
                    }
                }
            };

            string[] chatContext = relevancePosts
                .OrderByDescending(relevancePost => relevancePost.SimilarityScore)
                .Take(_limit)
                .Select(post => post.EmbeddingPost.RawText)
                .ToArray();

            await foreach (var output in _chatbot.GetAnswerAsync(request.Message, string.Join("\n\n", chatContext)))
            {
                yield return output;
            };
        }
    }

    public record RelevancePost
    {
        public required EmbeddingPost EmbeddingPost { get; set; }
        public required double SimilarityScore { get; set; }
    }
}