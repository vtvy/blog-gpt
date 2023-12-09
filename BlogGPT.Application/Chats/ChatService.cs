using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Application.Common.Interfaces.Services;
using System.Numerics.Tensors;
using System.Text;
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
        private IUser _user;
        public ChatService(IChatbot chatbot, IApplicationDbContext context, IUser user)
        {
            _context = context;
            _chatbot = chatbot;
            _user = user;
        }

        public async IAsyncEnumerable<string> SendStreamingAsync(ChatRequest request)
        {
            var minRelevanceScore = 0.7f;
            var limit = 3;

            var embeddingQuestion = _chatbot.GetEmbeddings([request.Message.Replace("?", "").Replace("\n", " ").Trim()])[0];

            List<RelevanceChunk> relevancePosts = [];

            await foreach (var relevanceChunk in _context.EmbeddingChunks
                .Select(embeddingChunk => new RelevanceChunk
                {
                    EmbeddingChunk = embeddingChunk,
                    PostId = embeddingChunk.PostId
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
                }
            };

            var mostRelevancePosts = relevancePosts
                .OrderByDescending(relevancePost => relevancePost.SimilarityScore)
                .Select(relevancePost => relevancePost.PostId)
                .Take(limit).ToArray();

            StringBuilder answer = new StringBuilder();
            var returnAnswer = "Sorry, I cannot find the relevance document base on your question.";

            if (mostRelevancePosts != null)
            {
                List<HistoryContext> histories = new List<HistoryContext>();
                float minRelevanceHistoryScore = 0.5f;
                await foreach (var relevanceMess in _context.Messages.Where(m => m.CreatedAt > DateTime.Today && m.AuthorId == _user.Id)
                    .AsNoTracking()
                    .AsAsyncEnumerable())
                {
                    if (relevanceMess != null)
                    {
                        var similarityScore = TensorPrimitives.CosineSimilarity(
                            new ReadOnlyMemory<float>(embeddingQuestion).Span,
                            new ReadOnlyMemory<float>(JsonSerializer.Deserialize<float[]>(relevanceMess.Embedding)).Span);

                        if (similarityScore >= minRelevanceHistoryScore)
                        {
                            histories.Add(new HistoryContext
                            {
                                Question = relevanceMess.Question,
                                Answer = relevanceMess.Answer,
                                SimilarityScore = similarityScore
                            });
                        }
                    }
                }

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
                    answer.Append(returnAnswer);
                    yield return returnAnswer;
                }
                else
                {
                    var articlesArray = chatContexts.Select(context => "Ariticle: " + context.Title + "\n" + context.RawText).ToArray();
                    var articles = string.Join("\n", articlesArray);

                    var history = histories.OrderByDescending(h => h.SimilarityScore).FirstOrDefault();
                    string previousHistory = "";
                    if (history != null)
                    {
                        previousHistory = $"""
                            <|user|>
                            {history.Question}</s>
                            <|assistant|>
                            {history.Answer}
                            """;
                    }

                    await foreach (var output in _chatbot.GetAnswerAsync(request.Message, articles, previousHistory))
                    {
                        answer.Append(output);
                        yield return output;
                    };
                    var sources = string.Join(", ", chatContexts.Select(chatContext => $"<a href=\"/posts/{chatContext.Slug}.html\" target=\"_blank\">{chatContext.Title}</a>").ToArray());
                    var returnSources = "Answered based on: " + sources;
                    yield return returnSources;
                }
            }
            else
            {
                answer.Append(returnAnswer);
                yield return returnAnswer;
            }
            if (answer.Length > 0)
            {
                var message = new Message
                {
                    Question = request.Message,
                    Answer = answer.ToString(),
                    Embedding = JsonSerializer.Serialize(embeddingQuestion),
                };
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
        }
    }

    public record RelevanceChunk
    {
        public int PostId { get; set; }
        public EmbeddingChunk EmbeddingChunk { get; set; } = null!;
        public double SimilarityScore { get; set; }
    }

    public record DocumentContext
    {
        public required string Title { get; set; }
        public required string RawText { get; set; }
        public required string Slug { get; set; }
    }

    public record HistoryContext
    {
        public required string Question { get; set; }
        public required string Answer { get; set; }
        public double SimilarityScore { get; set; }
    }
}