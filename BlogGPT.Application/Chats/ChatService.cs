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
            var minRelevanceScore = 0.7f;
            var limit = 4;

            var embeddingQuestion = _chatbot.GetEmbeddings([request.Message.Replace("?", "").Replace("\n", " ").Trim()])[0];

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
                    var similarityScore = TensorPrimitives.CosineSimilarity(
                        new ReadOnlyMemory<float>(embeddingQuestion).Span,
                        new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(relevanceChunk.EmbeddingChunk.Embedding)).Span);

                    if (similarityScore >= minRelevanceScore)
                    {
                        relevanceChunk.SimilarityScore = similarityScore;
                        relevancePosts.Add(relevanceChunk);
                    }
                    if (relevanceChunk.EmbeddingChunk.EmbeddingPostId == 10)
                    {
                        await Console.Out.WriteLineAsync(similarityScore.ToString());
                    }
                    if (relevanceChunk.EmbeddingChunk.EmbeddingPostId == 999)
                    {
                        await Console.Out.WriteLineAsync(similarityScore.ToString());
                    }
                }
            };

            var mostRelevancePosts = relevancePosts
                .OrderByDescending(relevancePost => relevancePost.SimilarityScore)
                .Select(relevancePost => relevancePost.EmbeddingPostId)
                .Take(limit).ToArray();

            if (mostRelevancePosts != null)
            {
                //.Where(embeddingPost => embeddingPost.Id == mostRelevancePost.EmbeddingPostId)
                var chatContexts = await _context.EmbeddingPosts
                    .Where(em => mostRelevancePosts.Contains(em.Id))
                    .Select(embedP => new DocumentContext
                    {
                        Title = embedP.Post.Title,
                        RawText = embedP.Post.RawText,
                        Slug = embedP.Post.Slug,
                    })
                    .ToListAsync();

                if (chatContexts == null)
                {
                    yield return "Sorry, I cannot find the relevance document base on your question.";
                }
                else
                {
                    var articlesArray = chatContexts.Select(context => "Ariticle: " + context.Title + "\n" + context.RawText).ToArray();
                    var articles = string.Join("\n", articlesArray);
                    await foreach (var output in _chatbot.GetAnswerAsync(request.Message, articles))
                    {
                        yield return output;
                    };
                    var sources = string.Join(", ", chatContexts.Select(chatContext => $"<a href=\"/posts/{chatContext.Slug}.html\" target=\"_blank\">{chatContext.Title}</a>").ToArray());
                    yield return "Answered based on: " + sources;
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
        public required string Title { get; set; }
        public required string RawText { get; set; }
        public required string Slug { get; set; }
    }
}