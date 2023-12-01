namespace BlogGPT.Application.Common.Interfaces.Services
{
    public interface IChatbot
    {
        IList<float[]> GetEmbeddings(IList<string> texts);
        IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext);
    }
}