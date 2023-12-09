namespace BlogGPT.Application.Common.Interfaces.Services
{
    public interface IChatbot
    {
        List<float[]> GetEmbeddings(List<string> texts);
        List<List<float[]>> GetEmbeddingsList(List<List<string>> textsList);
        IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext, string history);
    }
}