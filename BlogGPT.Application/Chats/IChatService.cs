namespace BlogGPT.Application.Chats
{
    public interface IChatService
    {
        IAsyncEnumerable<string> SendStreamingAsync(ChatRequest request);
    }
}