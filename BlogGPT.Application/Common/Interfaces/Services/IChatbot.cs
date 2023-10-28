namespace BlogGPT.Application.Common.Interfaces.Services
{
    public interface IChatbot
    {
        float[] GetEmbedding(string text);
        IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext);
    }
}